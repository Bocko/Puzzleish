using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeightChecker : MonoBehaviour
{
    public TextMeshPro weightReadout;

    List<Collider> collidersInChecker;

    void Start()
    {
        collidersInChecker = new List<Collider>();
        weightReadout.text = "READY";
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("ScaleWeight"))
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
            weightReadout.text = "TOO MANY OBJECTS";
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("ScaleWeight"))
        {
            return;
        }

        collidersInChecker.Remove(other);
        if (collidersInChecker.Count == 0)
        {
            weightReadout.text = "READY";
        }
        else if (collidersInChecker.Count < 2)
        {
            weightReadout.text = collidersInChecker[0].GetComponent<ScaleWeight>().weight.ToString();
        }
        else
        {
            weightReadout.text = "TOO MANY OBJECTS";
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
