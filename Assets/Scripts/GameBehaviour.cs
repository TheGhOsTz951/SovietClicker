using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    public int scoreTextInterval;
    public float blinkTotTime;
    public float blinkTime;

    public TextMeshProUGUI scoreText;
    public AudioSource cykaFX;

    // Script che contiene solo data
    public GameData gameData;

    private string[] scoreTextWords = {"amazing", "gualtastic", "supreme", "another one", "our red", "nazi suck", "сука блять"};
    private bool isBlinking;

    // Per poter stoppare la coroutine
    private Coroutine blinkCoroutine;

    void Start()
    {
        blinkCoroutine = null;
        refresh();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void refresh()
    {
        if (gameData.getScore() <= 0)
        {
            scoreText.text = "Start";
        } else
        {
            // Attiva il blinking text raggiunto un determinato score
            if (gameData.getScore() % scoreTextInterval == 0)
            {
                // Check if the coroutine is active -- Piu o meno
                if (blinkCoroutine != null)
                {
                    StopCoroutine(blinkCoroutine);
                }

                scoreText.text = "Score: " + gameData.getScore();
                string tempText = scoreTextWords[Random.Range(0, scoreTextWords.Length)];

                blinkCoroutine = StartCoroutine(BlinkingText(blinkTotTime, blinkTime, tempText));
            } 
            
            // Se non sta in blinking text aggiorna normale
            if (!isBlinking)
            {
                scoreText.text = "Score: " + gameData.getScore();
            }
        }
    }

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
    }
}
