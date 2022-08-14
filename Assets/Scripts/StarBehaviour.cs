using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBehaviour : MonoBehaviour
{
    // Tempo durata animazione
    public float fxTime;

    public SpriteRenderer spriteStar;
    public Camera cam;
    public AudioSource clickFX;

    // Contiene i text al momento -- Servirà per i salvataggi
    public EndlessBehaviour gameBehaviour;

    // Serve per passare il click
    public ScoreSliderBehaviour multiplierBehaviour;

    // Ogni quanto tempo parte il codice in update
    private float updateTime;

    // Servono per ottimizzare alle diverse grandezze di schermo -- Da cambiare con un panel interno
    private float camHeight;
    private float camWidth;
    private float spriteHeight;
    private float spriteWidth;

    private float xRandRange;
    private float yRandRange;

    private bool clickable;
    private float timeT;

    private Vector3 newPos;

    // Serve per fermare la coroutine
    private Coroutine fadeCoroutine;

    void Start()
    {
        timeT = Time.time + updateTime;

        fadeCoroutine = null;
        clickable = true;

        camHeight = 2f * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
        spriteWidth = spriteStar.bounds.size.x / 2;
        spriteHeight = spriteStar.bounds.size.y / 2;

        // Because the cam is in the vector 0 0 0
        xRandRange = camWidth/2 - spriteWidth;
        yRandRange = camHeight/2 - spriteHeight;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timeT)
        {
            if (updateTime > fxTime)
            {
                // Fading effect with randomPosition
                if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
                fadeCoroutine = StartCoroutine(FadeCicle(0f, fxTime));

            } else
            {
                RandomPosition();
            }

            updateTime = GameData.getStarSpeed();
            timeT += updateTime;
        }
    }

    public void OnMouseDown()
    {
        if (Input.GetMouseButton(0) && clickable)
        {
            // Fading effect with randomPosition
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeCicle(0f, fxTime));
            //  RandomPosition();  sta dentro fade cicle

            // Aggiunge un click calcolando il suo tempo
            //multiplierBehaviour.AddClickTime();

            // Aggiunge tempo alla slider
            GameData.setCountdownT(GameData.getCountdownT() + GameData.getUpCountdownT());

            clickFX.Play();
            //    gameData.setScore(gameData.getScore() + (1 * gameData.getMultiplier()));
            //    gameBehaviour.refresh();

            // Update fix
            timeT = Time.time + updateTime;
        }
    }

    public void RandomPosition()
    {
        newPos = new Vector3(Random.Range(-xRandRange, xRandRange), Random.Range(-yRandRange, yRandRange), 0);
        transform.position = newPos;
    }
    
    // Animazione di fading da un colore ad un altro -- Da cambiare
    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = spriteStar.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            spriteStar.color = newColor;
            yield return null;
        }

        Color newColor2 = spriteStar.color;
        newColor2.a = aValue;
        spriteStar.color = newColor2;
    }

    // Animazione completa di FadeOut e FadeIn con in mezzo il RandomPosition()
    IEnumerator FadeCicle(float aValue, float aTime)
    {
        clickable = false;

        // Start and wait for FadeTo to finish
        yield return FadeTo(aValue, aTime);
        RandomPosition();

        aValue = 1-aValue;

        // Start and wait for FadeTo to finish
        clickable = true;
        yield return FadeTo(aValue, aTime);
    }
}
