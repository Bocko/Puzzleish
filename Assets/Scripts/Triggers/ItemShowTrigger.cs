using UnityEngine;

public class ItemShowTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(transform.gameObject);
        }
    }
}
