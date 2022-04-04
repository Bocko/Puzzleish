using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauseAndEffectTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Teleporting(other.GetComponent<PlayerCauseAndEffect>()));
        }
    }

    IEnumerator Teleporting(PlayerCauseAndEffect playerCnE)
    {
        yield return playerCnE.ExternalTeleport(.1f);

        yield return new WaitForSeconds(1);

        if (NotificationManager.instance != null)
        {
            yield return NotificationManager.instance.ShowNotificationSync("What the hell?!..", 1.5f);
        }

        yield return new WaitForSeconds(1);

        yield return playerCnE.ExternalTeleport(.1f);

        yield return new WaitForSeconds(1);

        if (NotificationManager.instance != null)
        {
            yield return NotificationManager.instance.ShowNotificationSync("Its like i have traveled forward in time.", 2);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
