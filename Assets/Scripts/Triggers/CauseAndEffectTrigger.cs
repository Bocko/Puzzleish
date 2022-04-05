using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauseAndEffectTrigger : MonoBehaviour
{
    bool played;
    public float teleportFadeTime = 0.1f;
    public float waitTimeAfterTeleport = 1f;
    public float firstMessageWaitTime = 2f;
    public float waitTimeAfterFirstMessage = 2f;
    public float secondMessageWaitTime = 3f;

    void Start()
    {
        played = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!played)
            {
                StartCoroutine(Teleporting(other.GetComponent<PlayerCauseAndEffect>()));
            }
        }
    }

    IEnumerator Teleporting(PlayerCauseAndEffect playerCnE)
    {
        played = true;

        yield return playerCnE.ExternalTeleport(teleportFadeTime);

        yield return new WaitForSeconds(waitTimeAfterTeleport);

        if (NotificationManager.instance != null)
        {
            yield return NotificationManager.instance.ShowNotificationSync("What the hell?!..", firstMessageWaitTime);
        }

        yield return new WaitForSeconds(waitTimeAfterFirstMessage);

        yield return playerCnE.ExternalTeleport(teleportFadeTime);

        yield return new WaitForSeconds(waitTimeAfterTeleport);

        if (NotificationManager.instance != null)
        {
            yield return NotificationManager.instance.ShowNotificationSync("Its like i have traveled forward in time.", secondMessageWaitTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
