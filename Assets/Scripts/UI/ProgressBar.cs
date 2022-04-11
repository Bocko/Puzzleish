using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ProgressBar : MonoBehaviour
{
    public float minimum;
    public float maximum;
    public float current;
    public Image mask;
    public Color barColor;
    /*
    void Update()
    {
        GetCurrentFill();
    }*/

    public void ChangeCurrent(float newCurrent)
    {
        current = newCurrent;
        GetCurrentFill();
    }

    void GetCurrentFill()
    {
        float currentOffset = current - minimum;
        float maximumOffset = maximum - minimum;
        float fillAmount = currentOffset / maximumOffset;
        mask.fillAmount = fillAmount;
        mask.color = barColor;
    }
}
