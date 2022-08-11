using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBehaviour : MonoBehaviour
{
    // Indica su quanti click deve fare la media di tempo
    public int clicksTimeRange;

    // Tempo che deve passare per il codice in update
    public float timeT;
    private float timeTStep;

    // Tempo durata animazione
    public float fxTime;

    public SpriteRenderer spriteStar;
    public Camera cam;
    public AudioSource clickFX;

    // Script che contiene solo data
    public GameData gameData;

    // Contiene i text al momento -- Servirà per i salvataggi
    public GameBehaviour gameBehaviour;

    // Gestione del click medio tempo
    private int actualClickArrayPos;
    private float[] clicksTime;

    private float timeClick;

    // Servono per ottimizzare alle diverse grandezze di schermo -- Da cambiare con un panel interno
    private float camHeight;
    private float camWidth;
    private float spriteHeight;
    private float spriteWidth;

    private float xRandRange;
    private float yRandRange;

    private bool clickable;

    private Vector3 newPos;

    // Serve per fermare la coroutine
    private Coroutine fadeCoroutine;

    void Start()
    {
        timeClick = 0;
        timeTStep = timeT;

        actualClickArrayPos = 0;
        clicksTime = new float[clicksTimeRange];

        fadeCoroutine = null;
        clickable = true;

        camHeight = 2f * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
        spriteWidth = spriteStar.bounds.size.x / 2;
        spriteHeight = spriteStar.bounds.size.y / 2;

        // Because the cam is in the vector 0 0 0
        xRandRange = camWidth/2 - spriteWidth;
        yRandRange = camHeight/2 - spriteHeight;

        // Set to 0 all lastClickTime value
        for (int i = 0; i < clicksTimeRange; i++) clicksTime[i] = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeClick += Time.deltaTime;

        if (Time.time > timeT) {
            Debug.LogWarning("Media: " + MediaClickTime());

            timeT += timeTStep;
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

            AddClickTime();
            clickFX.Play();
            gameData.setScore(gameData.getScore() + 1);
            gameBehaviour.refresh();
        }
    }

    public void RandomPosition()
    {
        newPos = new Vector3(Random.Range(-xRandRange, xRandRange), Random.Range(-yRandRange, yRandRange), 0);
        transform.position = newPos;
    }

    // Aggiunge il tempo passato dal click precedente a quello attuale all'array clicksTime
    private void AddClickTime()
    {
        if (actualClickArrayPos >= clicksTimeRange - 1) actualClickArrayPos = 0;

        clicksTime[actualClickArrayPos] = timeClick;

        actualClickArrayPos++;
        timeClick = 0;
    }

    // Esegue la media dell'array clickTime in correlazione col tempo dall'ultimo click
    // Non so quanto sia effettivamente vera, but sembra funzionante
    private float MediaClickTime()
    {
        int range = clicksTimeRange;
        float media = 0;
        

        for (int i = 0; i<clicksTimeRange; i++)
        {
            if (clicksTime[i] == 0)
            {
                range--;
            }

            media += clicksTime[i];
        }

        media = (media / range) * timeClick;

        return media;
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
