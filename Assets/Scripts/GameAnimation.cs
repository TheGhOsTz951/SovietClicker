using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAnimation : MonoBehaviour
{
    // Fading animation
    public static void FadeToAlpha(GameObject objToAnim, float newValue, float time)
    {
        LeanTween.cancel(objToAnim);

        LeanTween.alpha(objToAnim, newValue, time);
    }

    public static void FadeTo(FadeAnimation sliderAnim)
    {
        LeanTween.alphaCanvas(sliderAnim.objToAnim.GetComponent<CanvasGroup>(), sliderAnim.value, sliderAnim.animT);
    }

    public static void BlinkAlphaAnim(BlinkingAnim blinkingValues)
    {
        LeanTween.alphaCanvas(blinkingValues.objToAnim.GetComponent<CanvasGroup>(), blinkingValues.value, blinkingValues.animT)
            .setEase(LeanTweenType.easeInOutQuint)
            .setLoopPingPong(5);
    }

}
