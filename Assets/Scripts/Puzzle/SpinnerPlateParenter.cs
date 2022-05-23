using UnityEngine;

public class SpinnerPlateParenter : MonoBehaviour
{
    Transform originalParent;
    private string playerTag = "Player";
    private void OnTriggerEnter(Collider other)//adds the player as a child of the spinner so the player spins like the spinner....
    {
        if (other.CompareTag(playerTag))
        {
            originalParent = other.transform.parent;
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            other.transform.parent = originalParent;
        }
    }
}
