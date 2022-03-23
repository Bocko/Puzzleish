using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvLightIntensitySetter : MonoBehaviour
{
    public float lightIntensity = 1;

    void Awake()
    {
        RenderSettings.ambientIntensity = lightIntensity;
    }
}