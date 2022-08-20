﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Custom serializable class
[System.Serializable]
public class Animation
{
    public GameObject objToAnim;
    public CanvasGroup canvasGroup;
    public float value = 1;
    public float time = 1;
}

public class EndlessBehaviour : MonoBehaviour
{
    [Header("- Components")]
    public SpriteBehaviour spriteBehaviour;
    public TextMeshProUGUI thisGameTimeTxt;
    public TextMeshProUGUI bonusMulTxt;
    public AudioSource cykaFX;

    [Header("- Time")]
    public float countdownT;
    public float upCountdownT;
    [Tooltip("Ogni quanto tempo parte il codice in update")]
    public float updateTime;

    [Header("- Difficulty")]
    // Servono per decidere ogni quanto tempo aumenta la difficolta
    public float difficultyChangeT;
    private float diffChangeDeltaT;
    [Tooltip("Moltiplicatore di Up Countdown T")]
    public float difficultyUpCountMul;
    [Tooltip("Moltiplicatore di Mul Countdown T")]
    public float difficultyCountMul;
    public float startCountDownMul;
    public float mulCountDown;

    [Header("- Slider")]
    public float sliderDiffStep;

    [Header("- Animations")]
    public Animation onHitAnim;
    public Animation onMissAnim;

    [Header("- Other")]
    [Tooltip("Script che gestisce le scene")]
    public string newSceneName;
    public int scoreTextInterval;
    public float blinkTotTime;
    public float blinkTime;

    private float timeT;
    private float tempSpriteClick;
    private bool isClickable;

    void Start()
    {
        // Update every timeT
        timeT = Time.time + updateTime;

        // Difficulty handler
        diffChangeDeltaT = difficultyChangeT;
        
        // Time txt
        thisGameTimeTxt.text = GameMethods.FormatToTime(GameData.getThisGameT());

        // Sprite click
        isClickable = true;
        tempSpriteClick = 0;
        GameData.setSpriteClick(0);

        // Setting starting parameters about time
        GameData.setThisGameT(0);
        GameData.setCountdownT(countdownT);
        GameData.setUpCountdownT(upCountdownT);

        // Moltiplicator of countdown velocity
        mulCountDown = startCountDownMul;
        GameData.setMulCountDownT(startCountDownMul);

        // Bonus if rowClicking
        GameData.setBonusMul(1);
        bonusMulTxt.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        // Se qualcosa è stato cliccato nel campo 2d
        if (isClickChanged())
        {
            if (GameData.getSpriteClick() > tempSpriteClick) SpriteClicked();
            else SpriteMissed();

            //GameData.setBonusMul(1 + GameData.getRowClick() * 0.1f);
            tempSpriteClick = GameData.getSpriteClick();
        }

        // Se il countdown è finito
        if (GameData.getCountdownT() <= 0)
        {
            GameData.setCountdownT(0);
            
            if (GameData.getThisGameT() > GameData.getHighScore())
            {
                GameData.setHighScore(GameData.getThisGameT());
            }

            GameMethods.changeScene(newSceneName);
        } 
        // Se il countdown è attivo
        else
        {
          //  mulCountDown = GameData.getMulCountDownT() * ;
            GameData.setCountdownT(GameData.getCountdownT() - (Time.deltaTime * mulCountDown));
            GameData.setThisGameT(GameData.getThisGameT() + Time.deltaTime);
        }

        // Aumenta la difficoltà ogni T
        if (GameData.getThisGameT() > diffChangeDeltaT)
        {
            diffChangeDeltaT += difficultyChangeT;

            GameData.setUpCountdownT(GameData.getUpCountdownT() * difficultyUpCountMul);
            GameData.setMulCountDownT(GameData.getMulCountDownT() * difficultyCountMul);
        }

        if (Time.time > timeT)
        {
            thisGameTimeTxt.text = GameMethods.FormatToTime(GameData.getThisGameT());

            if (GameData.getBonusMul() > 1.1f) bonusMulTxt.text = "x" + GameData.getBonusMul().ToString("0.0");
            else bonusMulTxt.text = "";

            timeT += updateTime;
        }
    }
    
    // Restituisce se la variabile sprite click è cambiata -- 0 No  1 Up 2 Down
    // Se non è clickable return false
    private bool isClickChanged()
    {
        if (!isClickable) tempSpriteClick = GameData.getSpriteClick();

        if (GameData.getSpriteClick() != tempSpriteClick) return true;

        return false;
    }

    private void SpriteClicked()
    {
        isClickable = false;

        GameData.setRowClick(GameData.getRowClick() + 1);
        GameData.setCountdownT(GameData.getCountdownT() + GameData.getUpCountdownT() * GameData.getBonusMul());

        LeanTween.cancel(onHitAnim.objToAnim);
        LeanTween.alpha(onHitAnim.objToAnim, onHitAnim.value, onHitAnim.time)
            .setOnComplete(ChangePosAndAnim);
    }

    // Serve per far funzionare bene le anim
    private void ChangePosAndAnim()
    {
        spriteBehaviour.RandomPosition();

        LeanTween.alpha(onHitAnim.objToAnim, 1-onHitAnim.value, onHitAnim.time)
            .setOnCompleteParam(isClickable = true);
    }

    private void SpriteMissed()
    {
        GameData.setRowClick(GameData.getRowClick() - 2);

        // Se non esiste il gruppo di canva non fa l'anim
        if (onMissAnim.canvasGroup == null) return;

        // Press spamming bug handler
        LeanTween.cancel(onMissAnim.objToAnim);
        onMissAnim.canvasGroup.alpha = 0f;

        LeanTween.alphaCanvas(onMissAnim.canvasGroup, onMissAnim.value, onMissAnim.time)
            .setEasePunch();
    }
 





    /* Inutilizzato
    IEnumerator BlinkingText(float tDuration, float tFlash, string text)
    {
        isBlinking = true;
        scoreText.text = text;

        cykaFX.Play();

        for (int i = 0; i < tDuration/tFlash; i++)
        {
            if (scoreText.color.a == 1)
            {
                yield return new WaitForSeconds(tFlash);
                scoreText.color = new Color(1, 1, 1, 0);
            } else
            {
                yield return new WaitForSeconds(tFlash);
                scoreText.color = new Color(1, 1, 1, 1);
            }
        }

        scoreText.color = new Color(1, 1, 1, 1);
        scoreText.text = "Score: " + gameData.getScore();
        isBlinking = false;
    }*/
}