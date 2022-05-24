using UnityEngine;

public class SpinnerPlateParenter : MonoBehaviour
{
    Transform originalParent;

    private void OnTriggerEnter(Collider other)//adds the player as a child of the spinner so the player spins like the spinner....
    {
        if (other.CompareTag(Tags.PlayerTag))
        {
            originalParent = other.transform.parent;
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.PlayerTag))
        {
            other.transform.parent = originalParent;
        }
    }
}
