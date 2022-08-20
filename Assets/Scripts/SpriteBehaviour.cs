using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpriteBehaviour : MonoBehaviour
{
    [Header("- Objects")]
    public SpriteRenderer spriteStar;
    public Camera cam;
    public AudioSource clickFX;

    [Header("- Camera fix for UI")]
    public GameObject UiArea_Top;
    public GameObject UiArea_Bottom;

    [Header("- Other")]
    public bool ChangePosOnClick;

    // Servono per ottimizzare alle diverse grandezze di schermo
    private float camHeight;
    private float camWidth;
    private float spriteHeight;
    private float spriteWidth;

    // Range of star random position
    private float xRandRange;
    private float yRandRange;
    private float yTopFixed;
    private float yBottomFixed;

    void Start()
    {
        SetGameBounds();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider == null)
            {
                SpriteMissed();

            } else if (hit.collider.gameObject == spriteStar.gameObject)
            {
                SpriteClicked();
            }
        }
    }

    public void SpriteClicked()
    {
        GameData.setSpriteClick(GameData.getSpriteClick() + 1);

       // if (ChangePosOnClick) RandomPosition();
       // if (clickFX != null) clickFX.Play();
    }

    public void SpriteMissed()
    {
        GameData.setSpriteClick(GameData.getSpriteClick() - 1);
    }

    private void SetGameBounds()
    {
        camHeight = 2f * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
        spriteWidth = spriteStar.bounds.size.x / 2;
        spriteHeight = spriteStar.bounds.size.y / 2;

        // Because the cam is in the vector 0 0 0
        xRandRange = camWidth / 2 - spriteWidth;
        yRandRange = camHeight / 2 - spriteHeight;

        yTopFixed = yRandRange;
        yBottomFixed = yRandRange;

        if (UiArea_Top != null)
        {
            yTopFixed -= (UiArea_Top.GetComponent<RectTransform>().rect.height * cam.aspect) / 100;
        }

        if (UiArea_Bottom != null)
        {
            yBottomFixed -= (UiArea_Bottom.GetComponent<RectTransform>().rect.height * cam.aspect) / 100;
        }

    }

    public void RandomPosition()
    {
        Vector3 newPos = new Vector3(Random.Range(-xRandRange, xRandRange), Random.Range(-yBottomFixed, yTopFixed), 0);
        spriteStar.transform.position = newPos;
    }
    
    public void ChangePos(Vector3 newPos)
    {
        spriteStar.transform.position = newPos;
    }
}
