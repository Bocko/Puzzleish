using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMover : MonoBehaviour
{
    public float offset = 17.2f;
    public float laserTime = 5;
    void Start()
    {
        StartCoroutine(LaserMoving());
    }

    IEnumerator LaserMoving()
    {
        float percent = 0;
        float laserSpeed = 1 / laserTime;
        while (true)
        {
            percent += Time.deltaTime * laserSpeed;
            float z = Mathf.PingPong(percent, offset * 2) - offset;
            print(z);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);

            yield return null;
        }
    }
}
