using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private int score;
    private int multiplier;

    private void Start()
    {
        multiplier = 1;

        if (PlayerPrefs.HasKey("score"))
        {
            score = PlayerPrefs.GetInt("score");
        }
    }

    public int getScore()
    {
        return score;
    }

    public void setScore(int newScore)
    {
        score = newScore;
        PlayerPrefs.SetInt("score", getScore());
    }

    public void setMultiplier(int newMultiplier)
    {
        multiplier = newMultiplier;
    }

    public int getMultiplier()
    {
        return multiplier;
    }
}
