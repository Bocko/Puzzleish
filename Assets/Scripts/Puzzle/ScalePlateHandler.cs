using System.Collections.Generic;
using UnityEngine;

public class ScalePlateHandler : MonoBehaviour
{
    public event System.Action<float> WeightChange;
    List<Collider> weights = new List<Collider>();
    private readonly string scaleTag = "ScaleWeight";

    void OnTriggerEnter(Collider other)//sending event when a new cube is inside the plate trigger and adding it to a list
    {
        if (other.CompareTag(scaleTag))
        {
            weights.Add(other);
            WeightChange?.Invoke(other.GetComponent<ScaleWeight>().weight);
        }
    }

    void OnTriggerExit(Collider other)//sending event when a new cube is inside the plate trigger and removing it form the list
    {
        if (other.CompareTag(scaleTag))
        {
            weights.Remove(other);
            WeightChange?.Invoke(-other.GetComponent<ScaleWeight>().weight);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    public void AdjustWeightsOnPlate(float newPos)
    {
        foreach (Collider weight in weights)
        {
            Transform weightT = weight.transform.parent.transform;
            weightT.position = new Vector3(weightT.position.x, newPos, weightT.position.z);
        }
    }
}
