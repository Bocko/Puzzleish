using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackShowTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(transform.gameObject);
        }
    }
}
