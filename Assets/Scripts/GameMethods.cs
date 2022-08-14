using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMethods : MonoBehaviour
{
    public static void changeScene(string newSceneName)
    {
        SceneManager.LoadScene(newSceneName);
    }

    public static string FormatToTime(float num)
    {
        string res;
        int minutes = (int)Mathf.Floor(num / 60);
        int seconds = (int)(num - (minutes * 60));

        if (minutes < 10) res = "0" + minutes;
        else res = minutes.ToString();

        res += ":";

        if (seconds < 10) res += "0" + seconds;
        else res += seconds.ToString();

        return res;
    }
}
