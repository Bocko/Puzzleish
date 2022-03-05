using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReset : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -5)
        {
            transform.position = Vector3.up * 5;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
