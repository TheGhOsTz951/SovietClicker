using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Custom serializable class
[System.Serializable]
public class FadeAnimation
{
    public GameObject objToAnim;
    public CanvasGroup canvasGroup;
    public float value = 1;
    public float animT = 0.1f;
}

[System.Serializable]
public class BlinkingAnim
{
    public CanvasGroup objToAnim;
    public float value = 0;
    public float animT = 0.1f;
    public int blinkNum = 5;
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
    public float valueCap;
    public float diffAumento;
    public float scoreDiffAumento;
    private float valueDownPerSec;
    private float clickValueDownPerSec;
    private float diffMul;
    private float keepScoreDiffAumento;

    [Header("- Slider")]
    public float sliderDiffStep;

    [Header("- Animations")]
    public FadeAnimation onHitAnim;
    public FadeAnimation onMissAnim;
    public FadeAnimation endSliderFadeAnim;
    public FadeAnimation endSceneAnim;
    public BlinkingAnim endTxtBlinkAnim;

    [Header("- Other")]
    [Tooltip("Script che gestisce le scene")]
    public string newSceneName;
    public string endGameString;

    private float timeT;
    private float tempSpriteClick;
    private bool isClickable;

    private void Awake()
    {
        
    }

    void Start()
    {
        // Update every timeT
        timeT = Time.time + updateTime;

        // Know when the game is ended
        GameData.IsEndGame = false;

        // Difficulty handler
        diffMul = 1;
        valueDownPerSec = 1;
        clickValueDownPerSec = 1;
        keepScoreDiffAumento = scoreDiffAumento;
        
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

        // Bonus if rowClicking
        GameData.setBonusMul(1);
        bonusMulTxt.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.IsEndGame) return;

        // Se il countdown è finito
        if (GameData.getCountdownT() <= 0)
        {
            EndGame();
            return;
        } 
        // Se il countdown è attivo
        else
        {
            valueDownPerSec = CalcValueDown();

            // CountdownT è da vedere come una barra che va da 100 a 0
            GameData.setCountdownT(GameData.getCountdownT() - (Time.deltaTime * valueDownPerSec));
            GameData.setThisGameT(GameData.getThisGameT() + Time.deltaTime);
        }

        // Se qualcosa è stato cliccato nel campo 2d
        if (isClickChanged())
        {
            if (GameData.getSpriteClick() > tempSpriteClick) SpriteClicked();
            else SpriteMissed();

            tempSpriteClick = GameData.getSpriteClick();
        }


        // Aumenta la difficoltà ogni T
        if (GameData.getThisGameT() > scoreDiffAumento)
        {
            diffMul += diffAumento;
            scoreDiffAumento += keepScoreDiffAumento;
        }

        if (Time.time > timeT)
        {
            thisGameTimeTxt.text = GameMethods.FormatToTime(GameData.getThisGameT());

            if (GameData.getRowClick() > 1) bonusMulTxt.text = "x" + GameData.getRowClick().ToString("0");
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

    // Da modificare
    private void SpriteClicked()
    {
        isClickable = false;

        GameData.setRowClick(GameData.getRowClick() + 1);
        GameData.setCountdownT(GameData.getCountdownT() + GameData.getUpCountdownT() * GameData.getBonusMul());

        LeanTween.cancel(onHitAnim.objToAnim);
        LeanTween.alpha(onHitAnim.objToAnim, onHitAnim.value, onHitAnim.animT)
            .setOnComplete(ChangePosAndAnim);
    }

    // Serve per far funzionare bene le anim
    private void ChangePosAndAnim()
    {
        spriteBehaviour.RandomPosition();

        LeanTween.alpha(onHitAnim.objToAnim, 1-onHitAnim.value, onHitAnim.animT)
            .setOnCompleteParam(isClickable = true);
    }

    private void SpriteMissed()
    {
        GameData.setRowClick(Mathf.Ceil(GameData.getRowClick()/10));

        // Se non esiste il gruppo di canva non fa l'anim
        if (onMissAnim.canvasGroup == null) return;

        // Press spamming bug handler
        LeanTween.cancel(onMissAnim.objToAnim);
        onMissAnim.canvasGroup.alpha = 0f;

        LeanTween.alphaCanvas(onMissAnim.canvasGroup, onMissAnim.value, onMissAnim.animT)
            .setEasePunch();
    }

    private void EndGame()
    {
        // Game is ended
        GameData.IsEndGame = true;

        GameData.setCountdownT(0);

        if (GameData.getThisGameT() > GameData.getHighScore())
        {
            GameData.setHighScore(GameData.getThisGameT());
        }

        spriteBehaviour.ChangePos(new Vector3(0, 0, 0));
        GameAnimation.FadeTo(endSliderFadeAnim);

        thisGameTimeTxt.text = endGameString;
        GameAnimation.BlinkAlphaAnim(endTxtBlinkAnim);

        StartCoroutine(GameMethods.ChangeSceneAnim(endSceneAnim, newSceneName, (endTxtBlinkAnim.blinkNum * endTxtBlinkAnim.animT) * 2));
    }

    private float CalcValueDown()
    {
        float tempValueDown = diffMul * valueCap;

        if (GameData.getRowClick() > 0) clickValueDownPerSec = tempValueDown * diffMul / Mathf.Pow(GameData.getRowClick(), 1f / 4f);
        else clickValueDownPerSec = tempValueDown;

        if (clickValueDownPerSec < tempValueDown) tempValueDown = clickValueDownPerSec;

        Debug.Log(tempValueDown);
        return tempValueDown;
    }
}
