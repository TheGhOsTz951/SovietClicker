using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private static float countdownT;
    private static float upCountdownT;
    // Serve per farlo scendere di più secondi (viene moltiplicato con Time.deltaTime)
    private static float mulCountDownT;
    private static float thisGameT;

    private static float highscore;
    private static float starSpeed;

    private void Start() 
    { 
        if (PlayerPrefs.HasKey("highscore")) highscore = PlayerPrefs.GetFloat("highscore");
        else highscore = 0;
    }

    public static float getHighScore()
    {
        return highscore;
    }

    public static void setHighScore(float newHighScore)
    {
        highscore = newHighScore;
        PlayerPrefs.SetFloat("highscore", getHighScore());
    }

    public static float getCountdownT()
    {
        return countdownT;
    }

    public static void setCountdownT(float newCountdownT)
    {
        countdownT = newCountdownT;
    }

    public static float getUpCountdownT()
    {
        return upCountdownT;
    }

    public static void setUpCountdownT(float newUpCountdownT)
    {
        upCountdownT = newUpCountdownT;
    }

    public static float getThisGameT()
    {
        return thisGameT;
    }

    public static void setThisGameT(float newThisGameT)
    {
        thisGameT = newThisGameT;
    }

    public static float getStarSpeed()
    {
        return starSpeed;
    }

    public static void setStarSpeed(float newStarSpeed)
    {
        starSpeed = newStarSpeed;
    }

    public static float getMulCountDownT()
    {
        return mulCountDownT;
    }

    public static void setMulCountDownT(float newMulCountDownT)
    {
        mulCountDownT = newMulCountDownT;
    }
}
