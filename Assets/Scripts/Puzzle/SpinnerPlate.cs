using System.Collections;
using UnityEngine;

public class SpinnerPlate : MonoBehaviour
{
    public enum RotateDirection { CW, CCW }

    public float degreePerSec = 90;
    public RotateDirection dir = RotateDirection.CW;

    private bool spinnerEnabled;

    private void OnEnable()
    {
        spinnerEnabled = true;
        StartCoroutine(Spin());
    }

    private void OnDisable()
    {
        spinnerEnabled = false;
    }

    IEnumerator Spin()
    {
        while (spinnerEnabled)
        {
            transform.rotation *= Quaternion.Euler((dir == RotateDirection.CW ? 1 : -1) * degreePerSec * Time.deltaTime * Vector3.up);

            yield return null;
        }
    }
}
