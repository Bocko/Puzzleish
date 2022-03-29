using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackTrigger : MonoBehaviour
{
    public enum JetpackStatus { ON, OFF }

    public JetpackStatus statusToSetJetpack;

    void OnTriggerEnter(Collider other)
    {
        PlayerJetpack pjp = other.GetComponent<PlayerJetpack>();
        if (pjp != null)
        {
            if (pjp.IsOn == (statusToSetJetpack == JetpackStatus.ON)) return;

            pjp.IsOn = statusToSetJetpack == JetpackStatus.ON;
            NotificationManager.instance.ShowNotification(pjp.IsOn ? "JETPACK ENABLED!" : "JETPACK DISABLED!", 1);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
        Gizmos.DrawIcon(transform.position, "d_Spotlight Icon", false, statusToSetJetpack == JetpackStatus.ON ? Color.green : Color.red);
    }
}
