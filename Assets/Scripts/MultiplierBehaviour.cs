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

    // Slider Colors senza alfa
    public Color mulSliderBgColor;
    public Color mulSliderFillColor;

    // Fade animation time
    public float Fx_FadeTime;
    public float Fx_SliderValueTime;

    // Tempo che deve passare per il codice in update
    private float timeT;
    public float timeTStep;

    public float mulScoreUp;
    public float mulScoreDown;
    private float mulScore;

    public float mulGap;
    public float sliderAppearCap;
    private bool isVisible;

    // Start is called before the first frame update
    void Start()
    {
        mulScore = 0;
        timeT = 0;

        mulScoreText.text = "";

        isVisible = false;
        mulSliderBg.color = mulSliderBgColor;
        mulSliderFill.color = mulSliderFillColor;
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
            if (mulScore > (sliderAppearCap + mulScoreUp) && !isVisible)
            {
                isVisible = true;

                StartCoroutine(FadeTo(1, Fx_FadeTime, mulSliderBg));
                StartCoroutine(FadeTo(1, Fx_FadeTime, mulSliderFill));

            } else if (mulScore < sliderAppearCap && isVisible)
            {
                isVisible = false;

                StartCoroutine(FadeTo(0, Fx_FadeTime, mulSliderFill));
                StartCoroutine(FadeTo(0, Fx_FadeTime, mulSliderBg));
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
            if (mulScore > sliderAppearCap + mulScoreUp)
            {
                gameData.setMultiplier(((int)Mathf.Floor((mulScore - sliderAppearCap) / mulGap)) + 1);
                mulScoreText.text = "x" + gameData.getMultiplier();
            } else
            {
                gameData.setMultiplier(1);
                mulScoreText.text = "";
            }

            // Change color on slider percentage
            Color tempColor = Color.Lerp(Color.red, Color.yellow, mulSlider.value / 100);
            tempColor.a = mulSliderFill.color.a;
            mulSliderFill.color = tempColor;



            timeT += timeTStep;
        }
    }

    // Aggiunge il tempo passato dal click precedente a quello attuale all'array clicksTime
    public void AddClickTime()
    {
        mulScore += mulScoreUp;
    }

    // Animazione di fading
    IEnumerator FadeTo(float aValue, float aTime, Image image)
    {
        float alpha = image.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = image.color;
            newColor.a = Mathf.Lerp(alpha, aValue, t);
            image.color = newColor;

            yield return null;
        }

        Color newColor2 = image.color;
        newColor2.a = aValue;
        image.color = newColor2;
    }


}
