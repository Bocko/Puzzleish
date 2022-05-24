using System.Collections;
using UnityEngine;

public class CauseAndEffectTrigger : MonoBehaviour
{
    [Header("Settings")]
    public float teleportFadeTime = 0.1f;
    public float waitTimeAfterTeleport = 1f;
    public float firstMessageWaitTime = 2f;
    public float waitTimeAfterFirstMessage = 2f;
    public float secondMessageWaitTime = 3f;

    [Header("Tag")]
    public string playerTag = "Player";

    [Header("Texts")]
    public string firstMessageText = "What the hell?!..";
    public string secondMessageText = "Its like I have traveled forward in time.";

    WaitForSeconds waitAfterTeleport;
    WaitForSeconds waitAfterMessage;
    bool played;

    void Start()
    {
        played = false;
        waitAfterTeleport = new WaitForSeconds(waitTimeAfterTeleport);
        waitAfterMessage = new WaitForSeconds(waitTimeAfterFirstMessage);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
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

        yield return waitAfterTeleport;

        if (NotificationManager.instance != null)
        {
            yield return NotificationManager.instance.ShowNotificationSync(firstMessageText, firstMessageWaitTime);
        }

        yield return waitAfterMessage;

        yield return playerCnE.ExternalTeleport(teleportFadeTime);

        yield return waitAfterTeleport;

        if (NotificationManager.instance != null)
        {
            yield return NotificationManager.instance.ShowNotificationSync(secondMessageText, secondMessageWaitTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
