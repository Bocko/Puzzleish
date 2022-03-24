using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCloser : MonoBehaviour
{
    public DoorHandler doorToClose;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorToClose.SetState(DoorHandler.state.CLOSED);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
