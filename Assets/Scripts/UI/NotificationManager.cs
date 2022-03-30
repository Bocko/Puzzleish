using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager instance;
    public Transform notificationCanvas;
    public RectTransform notification;
    public float upperPosition = 50;
    public float lowerPosition = -50;
    public float moveTime = 0.25f;

    List<RectTransform> texts = new List<RectTransform>();

    void Awake()
    {
        instance = this;
    }

    public void ShowNotification(string textToShow, float delayTime)
    {
        StartCoroutine(AnimateNotification(textToShow, delayTime));
    }

    IEnumerator AnimateNotification(string textToShow, float delayTime)
    {
        RectTransform noti = Instantiate(notification, notificationCanvas);
        TextMeshProUGUI text = noti.GetChild(1).GetComponent<TextMeshProUGUI>();
        text.text = textToShow;

        float percent = 0;
        float moveSpeed = 1 / moveTime;
        int dir = 1;

        float endDelayTime = Time.time + moveTime + delayTime;

        while (percent >= 0)
        {
            percent += Time.deltaTime * moveSpeed * dir;

            if (percent >= 1)
            {
                percent = 1;
                if (Time.time > endDelayTime)
                {
                    dir = -1;
                }
            }

            noti.anchoredPosition = Vector2.up * Mathf.Lerp(upperPosition, lowerPosition, percent);
            yield return null;
        }

        Destroy(noti.gameObject);
    }
}
