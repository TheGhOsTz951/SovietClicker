using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuBehaviour : MonoBehaviour
{
    public TextMeshProUGUI highscoreText;
    public FadeAnimation awakeAnim;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("highscore")) GameData.setHighScore(PlayerPrefs.GetFloat("highscore"));
        else GameData.setHighScore(0);

        if (GameData.getHighScore() > 0)
        {
            highscoreText.text = "Highscore: " + GameMethods.FormatToTime(GameData.getHighScore());
        } else
        {
            highscoreText.text = "No highscore commander";
        }
    }

    private void Awake()
    {
        awakeAnim.canvasGroup.alpha = 0;
        GameAnimation.FadeTo(awakeAnim);
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
