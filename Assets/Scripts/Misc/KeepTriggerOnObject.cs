using UnityEngine;

public class KeepTriggerOnObject : MonoBehaviour
{
    Transform trigger;
    void Start()
    {
        trigger = transform.GetChild(0);
    }

    void Update()
    {
        if (trigger.localPosition != Vector3.zero)
        {
            trigger.localPosition = Vector3.zero;
        }
    }
}
