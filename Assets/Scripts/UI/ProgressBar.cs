using UnityEngine;
using UnityEngine.UI;

//source: https://www.youtube.com/watch?v=J1ng1zA3-Pk
public class ProgressBar : MonoBehaviour
{
    public float minimum;
    public float maximum;
    public float current;
    public Image mask;
    public Color barColor;

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
