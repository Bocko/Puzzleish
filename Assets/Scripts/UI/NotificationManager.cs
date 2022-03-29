using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager instance;
    public RectTransform notification;
    public TextMeshProUGUI notiText;
    public float upperPosition = 50;
    public float lowerPosition = -50;
    public float moveTime = 0.25f;

    void Awake()
    {
        instance = this;
    }

    public void ShowNotification(string textToShow,float delayTime)
    {
        notiText.text = textToShow;
        StartCoroutine(AnimateNotification(delayTime));
    }

    IEnumerator AnimateNotification(float delayTime)
    {
        float percent = 0;
        float moveSpeed = 1 / moveTime;
        int dir = 1;

        float endDelayTime = Time.time + moveTime + delayTime;

        while (percent >= 0)
        {
            percent += Time.deltaTime * moveSpeed * dir;

            if(percent >= 1)
            {
                percent = 1;
                if(Time.time > endDelayTime)
                {
                    dir = -1;
                }
            }

            notification.anchoredPosition = Vector2.up * Mathf.Lerp(upperPosition, lowerPosition, percent);
            yield return null;
        }
    }
}
