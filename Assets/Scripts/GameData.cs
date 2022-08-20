using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    // Star related
    private static int spriteClick;

    // End game
    private static bool isEndGame;

    private static float countdownT;
    private static float upCountdownT;
    // Serve per farlo scendere di più secondi (viene moltiplicato con Time.deltaTime)
    private static float mulCountDownT;
    private static float thisGameT;
    private static float bonusMul;
    private static float rowClick;

    private static float timeSliderMax;

    private static float highscore;

    public static bool IsEndGame { get => isEndGame; set => isEndGame = value; }

    public static int getSpriteClick()
    {
        return spriteClick;
    }

    public static void setSpriteClick(int newSpriteClick)
    {
        spriteClick = newSpriteClick;
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

    public static float getMulCountDownT()
    {
        return mulCountDownT;
    }

    public static void setMulCountDownT(float newMulCountDownT)
    {
        mulCountDownT = newMulCountDownT;
    }

    public static float getBonusMul()
    {
        return bonusMul;
    }

    public static void setBonusMul(float newBonusMul)
    {
        bonusMul = newBonusMul;
    }

    public static float getRowClick()
    {
        return rowClick;
    }

    public static void setRowClick(float newRowClick)
    {
        if (newRowClick < 0) newRowClick = 0;

        rowClick = newRowClick;
    }

    public static float getTimeSliderMax()
    {
        return timeSliderMax;
    }

    public static void setTimeSliderMax(float newTimeSliderMax)
    {
        timeSliderMax = newTimeSliderMax;
    }
}
