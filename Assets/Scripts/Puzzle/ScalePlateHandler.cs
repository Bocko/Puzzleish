using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePlateHandler : MonoBehaviour
{
    public event System.Action<float> WeightChange;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ScaleWeight"))
        {
            WeightChange?.Invoke(other.GetComponent<ScaleWeight>().weight);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ScaleWeight"))
        {
            WeightChange?.Invoke(-other.GetComponent<ScaleWeight>().weight);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
