using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 panelLoc;
    public float percThreshold = 0.2f;
    public float easing = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        panelLoc = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // SCREEN WIDTH... MA DIOPORCO
        float perc = (eventData.pressPosition.x - eventData.position.x) / Screen.width;

        if (Mathf.Abs(perc) >= percThreshold)
        {
            Vector3 newLocation = panelLoc;

            if (perc > 0)
            {
                newLocation += new Vector3(-Screen.width, 0, 0);
            } else if (perc < 0)
            {
                newLocation += new Vector3(Screen.width, 0, 0);
            }

            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            transform.position = newLocation;
            panelLoc = newLocation;
        } else
        {
            StartCoroutine(SmoothMove(transform.position, panelLoc, easing));
        }
    }

    IEnumerator SmoothMove(Vector3 startPos, Vector3 endPos, float s)
    {
        float t = 0.0f;

        while(t <= 1.0)
        {
            t += Time.deltaTime / s;
            transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }
}
