using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepTriggerOnObject : MonoBehaviour
{
    Transform trigger;
    void Start()
    {
        trigger = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(trigger.localPosition != Vector3.zero)
        {
            print("corrected");
            trigger.localPosition = Vector3.zero;
        }
    }
}
