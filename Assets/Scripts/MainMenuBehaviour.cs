using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuBehaviour : MonoBehaviour
{
    public TextMeshProUGUI highscoreText;

    // Start is called before the first frame update
    void Start()
    {
        if (GameData.getHighScore() > 0)
        {
            highscoreText.text = "Highscore: " + GameMethods.FormatToTime(GameData.getHighScore());
        } else
        {
            highscoreText.text = "No highscore commander";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void refreshText()
    {
        highscoreText.text = GameMethods.FormatToTime(GameData.getHighScore());
    }
}
