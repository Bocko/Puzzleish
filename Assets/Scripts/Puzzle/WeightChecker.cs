using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeightChecker : MonoBehaviour
{
    public TextMeshPro weightReadout;
    public string scaleWeightTag = "ScaleWeight";
    public string readyText = "READY";
    public string tooManyText = "TOO MANY OBJECTS";

    List<Collider> collidersInChecker;

    void Start()
    {
        collidersInChecker = new List<Collider>();
        weightReadout.text = readyText;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(scaleWeightTag))
        {
            return;
        }

        collidersInChecker.Add(other);
        if (collidersInChecker.Count < 2)
        {
            weightReadout.text = other.GetComponent<ScaleWeight>().weight.ToString();
        }
        else
        {
            weightReadout.text = tooManyText;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(scaleWeightTag))
        {
            return;
        }

        collidersInChecker.Remove(other);
        if (collidersInChecker.Count == 0)
        {
            weightReadout.text = readyText;
        }
        else if (collidersInChecker.Count < 2)
        {
            weightReadout.text = collidersInChecker[0].GetComponent<ScaleWeight>().weight.ToString();
        }
        else
        {
            weightReadout.text = tooManyText;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
