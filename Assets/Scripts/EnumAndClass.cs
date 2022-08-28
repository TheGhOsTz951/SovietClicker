using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
