using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerPlate : MonoBehaviour
{
    public enum RotateDirection { CW, CCW }

    public float degreePerSec = 90;
    public RotateDirection dir = RotateDirection.CW;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spin());
    }

    IEnumerator Spin()
    {
        while (true)
        {
            transform.rotation *= Quaternion.Euler((dir == RotateDirection.CW ? 1 : -1) * degreePerSec * Time.deltaTime * Vector3.up);

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
    }
}
