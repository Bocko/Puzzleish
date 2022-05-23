using System.Collections;
using UnityEngine;

public class WaterMover : MonoBehaviour
{
    public float minScale = 5;
    public float maxScale = 7;
    private bool waveEnabled;

    void OnEnable()
    {
        waveEnabled = true;
        StartCoroutine(WaterWaving());
    }

    void OnDisable()
    {
        waveEnabled = false;
    }

    IEnumerator WaterWaving()
    {
        float percent = 0;
        while (waveEnabled)
        {
            percent += Time.deltaTime;

            transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(minScale, maxScale, Mathf.PingPong(percent, 1)), transform.localScale.z);
            yield return null;
        }
    }

}
