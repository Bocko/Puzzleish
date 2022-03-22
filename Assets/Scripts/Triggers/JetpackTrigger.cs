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
            pjp.IsOn = statusToSetJetpack == JetpackStatus.ON;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
        Gizmos.DrawIcon(transform.position, "d_Spotlight Icon", false, statusToSetJetpack == JetpackStatus.ON ? Color.green : Color.red);
    }
}
