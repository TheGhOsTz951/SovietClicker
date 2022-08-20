using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSliderBehaviour : MonoBehaviour
{
    [Header("-- Objects")]
    public Slider sliderObj;
    public Image mulSliderBg;
    public Image mulSliderFill;
    //public TextMeshProUGUI mulScoreText;

    [Header("-- Slider Related")]
    [Tooltip("Ogni quanto score deve aumentare di multiplier")]
    public float mulGap;
    [Tooltip("Punteggio necessario per far comparire la barra (ATTENZIONE! LAVORA CON MUL SCORE UP)")]
    public float sliderAppearCap;
    private bool isVisible;

    [Header("-- Slider Colors senza alfa")]
    public Color mulSliderBgColor;
    public Color mulSliderFillColor;
    
    [Header("-- Fade animation time")]
    public float Fx_FadeTime;
    public float Fx_SliderValueTime;

    [Header("-- Update time in seconds")]
    public float updateTime;
    private float timeT;
    
    [Header("-- MulScore Variables")]
    public float mulScoreUp;
    public float mulScoreDown;
    private float mulScore;

    // Start is called before the first frame update
    void Start()
    {
        timeT = Time.time + updateTime;

        GameData.setTimeSliderMax(GameData.getCountdownT() * 2);

        sliderObj.maxValue = GameData.getTimeSliderMax();
        sliderObj.minValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timeT)
        {
            if (GameData.getTimeSliderMax() > sliderObj.maxValue)
            {
                sliderObj.maxValue = GameData.getTimeSliderMax();
            }

            if (GameData.getCountdownT() > sliderObj.maxValue)
            {
                GameData.setCountdownT(sliderObj.maxValue);
                sliderObj.value = sliderObj.maxValue;

            } else if (GameData.getCountdownT() > sliderObj.minValue)
            {
                sliderObj.value = GameData.getCountdownT();

            } else
            {
                sliderObj.value = sliderObj.minValue;
            }

            // Change color on slider percentage
            Color tempColor = Color.Lerp(Color.red, Color.yellow, sliderObj.value / 100);
            tempColor.a = mulSliderFill.color.a;
            mulSliderFill.color = tempColor;

            timeT += updateTime;
        }
    }
}
