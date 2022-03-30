using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCauseAndEffect : MonoBehaviour
{
    public float offset = 20;
    public Vector3 direction = Vector3.right;
    public float effectTime = 0.1f;
    bool onLeft = true;
    CanvasGroup effect;

    void Start()
    {
        effect = GameObject.Find("teleportEffect").GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(Fade());
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

    IEnumerator Fade()
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
}
