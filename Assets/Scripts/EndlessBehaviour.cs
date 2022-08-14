using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndlessBehaviour : MonoBehaviour
{
    public int scoreTextInterval;
    public float blinkTotTime;
    public float blinkTime;

    public float countdownT;
    public float upCountdownT;

    // Servono per decidere ogni quanto tempo aumenta la difficolta
    public float difficultyChangeS;
    public float speedMul;
    public float baseStarSpeed;
    [Tooltip("Moltiplicatore di Up Countdown T")]
    public float difficultyUpCountMul;
    [Tooltip("Moltiplicatore di Mul Countdown T")]
    public float difficultyCountMul;

    public TextMeshProUGUI thisGameTimeTxt;
    public AudioSource cykaFX;

    // Ogni quanto tempo parte il codice in update
    public float updateTime;

    // Script che gestisce le scene
    public string newSceneName;

    private string[] scoreTextWords = {"amazing", "gualtastic", "supreme", "another one", "our red", "nazi suck", "сука блять"};
    private bool isBlinking;

    // Per poter stoppare la coroutine
    private Coroutine blinkCoroutine;

    private float timeT;

    void Start()
    {
        timeT = Time.time + updateTime;

        GameData.setThisGameT(0);
        thisGameTimeTxt.text = GameMethods.FormatToTime(GameData.getThisGameT());

        GameData.setCountdownT(countdownT);
        GameData.setUpCountdownT(upCountdownT);
        GameData.setMulCountDownT(1);
        GameData.setStarSpeed(baseStarSpeed);

        //    blinkCoroutine = null;
        //    refresh();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.getCountdownT() <= 0)
        {
            GameData.setCountdownT(0);
            
            if (GameData.getThisGameT() > GameData.getHighScore())
            {
                GameData.setHighScore(GameData.getThisGameT());
            }

            GameMethods.changeScene(newSceneName);
        } else
        {
            GameData.setCountdownT(GameData.getCountdownT() - (Time.deltaTime * GameData.getMulCountDownT()));
            GameData.setThisGameT(GameData.getThisGameT() + Time.deltaTime);
        }

        if (GameData.getThisGameT() > difficultyChangeS)
        {
            difficultyChangeS += GameData.getThisGameT();

            GameData.setStarSpeed(GameData.getStarSpeed() * speedMul);
            GameData.setUpCountdownT(GameData.getUpCountdownT() * difficultyUpCountMul);
            GameData.setMulCountDownT(GameData.getMulCountDownT() * difficultyCountMul);
        }

        if (Time.time > timeT)
        {
            thisGameTimeTxt.text = GameMethods.FormatToTime(GameData.getThisGameT());

            timeT += updateTime;
        }
    }

    public void AnotherRefresh()
    {

    }

    

    /* Inutilizzato
    public void refresh()
    {
        if (GameData.getScore() <= 0)
        {
            scoreText.text = "Start";
        } else
        {
            // Attiva il blinking text raggiunto un determinato score -- DA CAMBIARE
            if (GameData.getScore() % scoreTextInterval == 0)
            {
                // Check if the coroutine is active -- Piu o meno
                if (blinkCoroutine != null)
                {
                    StopCoroutine(blinkCoroutine);
                }

                scoreText.text = "Score: " + GameData.getScore();
                string tempText = scoreTextWords[Random.Range(0, scoreTextWords.Length)];

                blinkCoroutine = StartCoroutine(BlinkingText(blinkTotTime, blinkTime, tempText));
            } 
            
            // Se non sta in blinking text aggiorna normale
            if (!isBlinking)
            {
                scoreText.text = "Score: " + GameData.getScore();
            }
        }
    }*/

    /* Inutilizzato
    IEnumerator BlinkingText(float tDuration, float tFlash, string text)
    {
        isBlinking = true;
        scoreText.text = text;

        cykaFX.Play();

        for (int i = 0; i < tDuration/tFlash; i++)
        {
            if (scoreText.color.a == 1)
            {
                yield return new WaitForSeconds(tFlash);
                scoreText.color = new Color(1, 1, 1, 0);
            } else
            {
                yield return new WaitForSeconds(tFlash);
                scoreText.color = new Color(1, 1, 1, 1);
            }
        }

        scoreText.color = new Color(1, 1, 1, 1);
        scoreText.text = "Score: " + gameData.getScore();
        isBlinking = false;
    }*/
}
