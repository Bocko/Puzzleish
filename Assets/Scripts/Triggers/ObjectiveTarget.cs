using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveTarget : MonoBehaviour
{
    public bool objectiveCompleted;
    public GameObject objectiveItem;
    public DoorOpener doorOpener;

    void OnTriggerEnter(Collider other)
    {
        if (CheckObjective(other.gameObject))
        {
            print("objetive complete");
            objectiveCompleted = true;
            doorOpener.SetState(DoorOpener.state.OPEN);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (CheckObjective(other.gameObject))
        {
            print("objetive incomplete");
            objectiveCompleted = false;
            doorOpener.SetState(DoorOpener.state.CLOSED);
        }
    }

    bool CheckObjective(GameObject other)
    {
        return ReferenceEquals(other, objectiveItem);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
