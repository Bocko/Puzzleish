using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScaleManager : MonoBehaviour
{
    public TextMeshPro middleText;
    public TextMeshPro leftText;
    public TextMeshPro rightText;
    public DoorHandler doorHandler;
    public float weightLimitPerPlate = 20;
    public ScalePlateHandler leftPlateHandler;
    public ScalePlateHandler rightPlateHandler;
    public Transform leftPlateHolder;
    public Transform rightPlateHolder;
    public Transform middleHandle;

    float leftPlateSum = 0;
    float rightPlateSum = 0;

    void Start()
    {
        UpdateTexts();
        leftPlateHandler.WeightChange += OnLeftPlateChange;
        rightPlateHandler.WeightChange += OnRightPlateChange;
    }

    void OnLeftPlateChange(float change)
    {
        leftPlateSum += change;
        UpdateTexts();
        CheckForPuzzleCompletion();
    }

    void OnRightPlateChange(float change)
    {
        rightPlateSum += change;
        UpdateTexts();
        CheckForPuzzleCompletion();
    }

    void CheckForPuzzleCompletion()
    {
        if (leftPlateSum == weightLimitPerPlate && rightPlateSum == weightLimitPerPlate)
        {
            doorHandler.SetState(DoorHandler.state.OPEN);
        }
        else
        {
            doorHandler.SetState(DoorHandler.state.CLOSED);
        }

    }

    void UpdateTexts()
    {
        leftText.text = (leftPlateSum - weightLimitPerPlate).ToString();
        rightText.text = (rightPlateSum - weightLimitPerPlate).ToString();

        if (leftPlateSum == rightPlateSum)
        {
            middleText.text = "==";
        }
        else if (leftPlateSum < rightPlateSum)
        {
            middleText.text = "<";
        }
        else
        {
            middleText.text = ">";
        }
    }
}
