using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerPlateParenter : MonoBehaviour
{
    Transform originalParent;
    private void OnTriggerEnter(Collider other)//adds the player as a child of the spinner so the player spins like the spinner....
    {
        if (other.CompareTag("Player"))
        {
            originalParent = other.transform.parent;
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = originalParent;
        }
    }
}
