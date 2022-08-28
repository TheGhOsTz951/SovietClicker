using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSliderBehaviour : MonoBehaviour
{
    [Header("-- Objects")]
    public Slider sliderObj;
    public Image mulSliderFill;

    [Header("-- Slider Colors senza alfa")]
    public Color mulSliderFillColor;

    [Header("-- Update time in seconds")]
    public float updateTime;
    private float timeT;

    // Start is called before the first frame update
    void Start()
    {
        timeT = Time.time + updateTime;

        sliderObj.maxValue = GameData.GameValue*2;
        sliderObj.minValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
       // if (Time.time > timeT)
       // {
            if (GameData.GameValue > sliderObj.maxValue)
            {
                GameData.GameValue = sliderObj.maxValue;
                sliderObj.value = sliderObj.maxValue;

            } else if (GameData.GameValue > sliderObj.minValue)
            {
                sliderObj.value = GameData.GameValue;

            } else
            {
                sliderObj.value = sliderObj.minValue;
            }

            // Change color on slider percentage
            Color tempColor = Color.Lerp(Color.red, Color.yellow, sliderObj.value / 100);
            tempColor.a = mulSliderFill.color.a;
            mulSliderFill.color = tempColor;

            timeT += updateTime;
       // }
    }
}
