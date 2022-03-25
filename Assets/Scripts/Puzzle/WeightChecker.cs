using System.Collections;
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
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ignore"))
        {
            return;
        }

        collidersInChecker.Add(other);
        if (collidersInChecker.Count < 2)
        {
            if (other.CompareTag("ScaleWeight"))
            {
                weightReadout.text = other.GetComponent<ScaleWeight>().weight.ToString();
            }
            else
            {
                weightReadout.text = "INVALID OBJECT";
            }
        }
        else
        {
            weightReadout.text = "TOO MANY OBJECTS";
        }

        foreach (var item in collidersInChecker)
        {
            print(item.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ignore"))
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
            if (collidersInChecker[0].CompareTag("ScaleWeight"))
            {
                weightReadout.text = collidersInChecker[0].GetComponent<ScaleWeight>().weight.ToString();
            }
            else
            {
                weightReadout.text = "INVALID OBJECT";
            }
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
