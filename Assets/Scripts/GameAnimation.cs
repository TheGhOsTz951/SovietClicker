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

    /* Animazione completa di FadeOut e FadeIn con in mezzo il RandomPosition()
    IEnumerator FadeCicle(float aValue, float aTime)
    {
        clickable = false;

        // Start and wait for FadeTo to finish
        yield return FadeTo(aValue, aTime);
        RandomPosition();

        aValue = 1 - aValue;

        // Start and wait for FadeTo to finish
        clickable = true;
        yield return FadeTo(aValue, aTime);
    }*/






    /*IEnumerator FailClickAnim()
    {
        isFailClickAnim = true;
        borderAnim.SetTrigger("Start");

        yield return new WaitForSeconds(borderAnimT);

        borderAnim.SetTrigger("End");

        yield return new WaitForSeconds(borderAnimT);
        isFailClickAnim = false;
    }*/
}
