using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCauseAndEffect : MonoBehaviour
{
    public float offset = 45;
    public Vector3 direction = Vector3.right;
    public float effectTime = 0.05f;
    public bool isOn = false;

    bool onLeft = true;
    CanvasGroup effect;

    void Start()
    {
        effect = GameObject.Find("teleportEffect").GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            bool timeTravelDown = false;
            if (Input.anyKeyDown)
            {
                timeTravelDown = Input.GetAxisRaw("TimeTravel") == 1;
            }

            if (timeTravelDown)
            {
                StartCoroutine(Fade(effectTime));
            }
        }
    }

    void Teleport()
    {
        if (onLeft)
        {
            transform.Translate(offset * 2 * direction, Space.World);
        }
        else
        {
            transform.Translate(offset * 2 * -direction, Space.World);
        }
        onLeft = !onLeft;
    }

    IEnumerator Fade(float effectTime)
    {
        float percent = 0;
        float effectSpeed = 1 / effectTime;
        float dir = 1;

        while (percent >= 0)
        {
            percent += Time.deltaTime * effectSpeed * dir;

            if(percent >= 1)
            {
                percent = 1;
                dir = -1;
                Teleport();
            }

            effect.alpha = percent;

            yield return null;
        }
    }

    public IEnumerator ExternalTeleport(float effectTime)
    {
        yield return StartCoroutine(Fade(effectTime));
    }
}
