using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndlessBehaviour : MonoBehaviour
{
    [Header("- Components")]
    public SpriteBehaviour spriteBehaviour;
    public TextMeshProUGUI thisGameTimeTxt;
    public TextMeshProUGUI rowClickTxt;
    public AudioSource cykaFX;

    [Header("- Game related")]
    public float thisGameValue;
    public float AddGameValue;
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
    private int rowClick;
    private float thisGameT;

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
        thisGameT = 0;
        thisGameTimeTxt.text = GameMethods.FormatToTime(thisGameT);

        // Sprite click
        isClickable = true;
        tempSpriteClick = 0;
        GameData.setSpriteClick(0);

        // Setting starting parameters about time
        GameData.GameValue = thisGameValue;

        // RowClick Mul
        rowClickTxt.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.IsEndGame) return;

        // Se il countdown è finito
        if (GameData.GameValue <= 0)
        {
            EndGame();
            return;
        } 
        // Se il countdown è attivo
        else
        {
            valueDownPerSec = CalcValueDown();

            // CountdownT è da vedere come una barra che va da 100 a 0
            GameData.GameValue = GameData.GameValue - (Time.deltaTime * valueDownPerSec);
            thisGameT += Time.deltaTime;
        }

        // Se qualcosa è stato cliccato nel campo 2d
        if (isClickChanged())
        {
            if (GameData.getSpriteClick() > tempSpriteClick) SpriteClicked();
            else SpriteMissed();

            tempSpriteClick = GameData.getSpriteClick();
        }


        // Aumenta la difficoltà ogni T
        if (thisGameT > scoreDiffAumento)
        {
            diffMul += diffAumento;
            scoreDiffAumento += keepScoreDiffAumento;
        }

        if (Time.time > timeT)
        {
            thisGameTimeTxt.text = GameMethods.FormatToTime(thisGameT);

            if (rowClick > 1) rowClickTxt.text = "x" + rowClick.ToString("0");
            else rowClickTxt.text = "";

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

        rowClick += 1;
        GameData.GameValue = GameData.GameValue + AddGameValue;

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
        rowClick = (int)Mathf.Ceil(rowClick/10);
        if (rowClick < 0) rowClick = 0;

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

        GameData.GameValue = 0;

        if (thisGameT > GameData.getHighScore())
        {
            GameData.setHighScore(thisGameT);
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

        if (rowClick > 0) clickValueDownPerSec = tempValueDown * diffMul / Mathf.Pow(rowClick, 1f / 4f);
        else clickValueDownPerSec = tempValueDown;

        if (clickValueDownPerSec < tempValueDown) tempValueDown = clickValueDownPerSec;

        return tempValueDown;
    }
}
