using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiplierBehaviour : MonoBehaviour
{
    public GameData gameData;
    public Slider mulSlider;
    public Image mulSliderBg;
    public Image mulSliderFill;
    public TextMeshProUGUI mulScoreText;

    public Color mulSliderBgColor;
    public Color mulSliderFillColor;

    // Tempo che deve passare per il codice in update
    private float timeT;
    public float timeTStep;

    public float mulScoreUp;
    public float mulScoreDown;
    private float mulScore;

    public float sliderAppearCap;
    private float maxSliderValue;
    private float minSliderValue;

    // Start is called before the first frame update
    void Start()
    {
        mulScore = 0;
        timeT = 0;

        mulScoreText.text = "";

        maxSliderValue = mulSlider.maxValue;
        minSliderValue = mulSlider.minValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timeT)
        {
            // > 0
            if (mulScore > 0)
            {
                mulScore -= mulScoreDown;
            }

            // Colori quando appaiono -- TODO WITH COROUTINE
            if (mulScore > sliderAppearCap)
            {
               // mulSliderBg.color = mulSliderBgColor;
               // mulSliderFill.color = mulSliderFillColor;
            } else
            {
               // mulSliderBg.color = new Color(1, 1, 1, 0);
               // mulSliderFill.color = new Color(1, 1, 1, 0);
            }

            // Bug < 0 handler
            if (mulScore <= 0) mulScore = 0;

            // Slider bug fix and max mulScore
            if (mulScore >= mulSlider.maxValue)
            {
               mulSlider.value = mulSlider.maxValue;
                mulScore = mulSlider.maxValue;
            }
            else
            {
                mulSlider.value = mulScore;
            }

            // Set multiplier -- TODO
            if (mulScore > 60)
            {
                gameData.setMultiplier(2);
                mulScoreText.text = "x2";
            } else
            {
                gameData.setMultiplier(1);
                mulScoreText.text = "";
            }

            // Change color on slider percentage
            mulSliderFill.color = Color.Lerp(Color.red, Color.yellow, mulSlider.value / 100);

            timeT += timeTStep;
        }
    }

    // Aggiunge il tempo passato dal click precedente a quello attuale all'array clicksTime
    public void AddClickTime()
    {
        mulScore += mulScoreUp;
    }
    
}
