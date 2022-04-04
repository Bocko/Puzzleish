using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRemover : MonoBehaviour
{
    public ObjectiveTarget obj;
    public GameObject door;
    bool completed;

    void Start()
    {
        completed = obj.objectiveCompleted;
    }

    void Update()
    {
        if (completed != obj.objectiveCompleted)
        {
            completed = obj.objectiveCompleted;
            door.SetActive(!completed);
        }
    }
}
