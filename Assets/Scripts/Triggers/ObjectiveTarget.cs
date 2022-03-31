using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveTarget : MonoBehaviour
{
    public bool objectiveCompleted;
    public GameObject objectiveItem;
    public DoorHandler doorHandler;
    public string objectiveName;

    void OnTriggerEnter(Collider other)
    {
        if (CheckObjective(other.gameObject))
        {
            print("objetive complete");
            objectiveCompleted = true;
            doorHandler.SetState(DoorHandler.state.OPEN);

            if(NotificationManager.instance != null)
            {
                NotificationManager.instance.ShowNotification($"{objectiveName} COMPLETED!", 1);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (CheckObjective(other.gameObject))
        {
            print("objetive incomplete");
            objectiveCompleted = false;
            doorHandler.SetState(DoorHandler.state.CLOSED);

            if (NotificationManager.instance != null)
            {
                NotificationManager.instance.ShowNotification($"{objectiveName} UNCOMPLETED!", 1);
            }
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
