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
    public float moveUnitPerWeight = 0.02f;
    public float rotationUnitPerWeight = 0.25f;
    public float defaultPlateHeight = 1;
    public float moveTime = 0.1f;

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
        StartCoroutine(MovePlate());
    }

    void OnRightPlateChange(float change)
    {
        rightPlateSum += change;
        UpdateTexts();
        CheckForPuzzleCompletion();
        StartCoroutine(MovePlate());
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

    IEnumerator MovePlate()
    {
        float dif = Mathf.Clamp(leftPlateSum - rightPlateSum, -40, 40);//clamped so no funky stuff with overloading the scale alright?
        float asd = Mathf.Abs(moveUnitPerWeight * dif);

        float percent = 0;
        float moveSpeed = 1 / moveTime;

        if (dif == 0)
        {
            while (percent < 1)
            {
                percent += Time.deltaTime * moveSpeed;

                leftPlateHolder.position = new Vector3(leftPlateHolder.position.x, Mathf.Lerp(leftPlateHolder.position.y, 1, percent), leftPlateHolder.position.z);
                rightPlateHolder.position = new Vector3(rightPlateHolder.position.x, Mathf.Lerp(rightPlateHolder.position.y, 1, percent), rightPlateHolder.position.z);
                middleHandle.rotation = Quaternion.Lerp(middleHandle.rotation, Quaternion.Euler(Vector3.zero), percent);

                yield return null;
            }
        }
        else if (dif > 0)
        {
            while (percent < 1)
            {
                percent += Time.deltaTime * moveSpeed;

                leftPlateHolder.position = new Vector3(leftPlateHolder.position.x, Mathf.Lerp(leftPlateHolder.position.y, 1 - asd, percent), leftPlateHolder.position.z);
                rightPlateHolder.position = new Vector3(rightPlateHolder.position.x, Mathf.Lerp(rightPlateHolder.position.y, 1 + asd, percent), rightPlateHolder.position.z);
                middleHandle.rotation = Quaternion.Lerp(middleHandle.rotation, Quaternion.Euler(0, 0, dif * rotationUnitPerWeight), percent);

                yield return null;
            }
        }
        else
        {
            while (percent < 1)
            {
                percent += Time.deltaTime * moveSpeed;

                leftPlateHolder.position = new Vector3(leftPlateHolder.position.x, Mathf.Lerp(leftPlateHolder.position.y, 1 + asd, percent), leftPlateHolder.position.z);
                rightPlateHolder.position = new Vector3(rightPlateHolder.position.x, Mathf.Lerp(rightPlateHolder.position.y, 1 - asd, percent), rightPlateHolder.position.z);
                middleHandle.rotation = Quaternion.Lerp(middleHandle.rotation, Quaternion.Euler(0, 0, dif * rotationUnitPerWeight), percent);

                yield return null;
            }
        }
    }
}
