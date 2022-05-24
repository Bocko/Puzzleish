using UnityEngine;

public class DoorCloser : MonoBehaviour
{
    [Header("Door Handler component")]
    public DoorHandler doorToClose;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.PlayerTag))
        {
            doorToClose.SetState(DoorHandler.state.CLOSED);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
