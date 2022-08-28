using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    // Star related
    private static int spriteClick;

    // End game
    private static bool isEndGame;

    // Valore che va da 100 a 0 ed equivale ad un contdown
    private static float gameValue;

    private static float highscore;

    public static bool IsEndGame { get => isEndGame; set => isEndGame = value; }
    public static float GameValue { get => gameValue; set => gameValue = value; }

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
}
