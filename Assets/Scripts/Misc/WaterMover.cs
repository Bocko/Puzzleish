using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMover : MonoBehaviour
{
    public float minScale = 5;
    public float maxScale = 7;

    void Start()
    {
        StartCoroutine(WaterWaving());
    }

    IEnumerator WaterWaving()
    {
        float percent = 0;
        while (true)
        {
            percent += Time.deltaTime;

            transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(minScale, maxScale, Mathf.PingPong(percent, 1)), transform.localScale.z);
            yield return null;
        }
    }

}
