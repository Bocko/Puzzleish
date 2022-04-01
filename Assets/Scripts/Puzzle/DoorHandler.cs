using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    public enum state { OPEN, CLOSED }
    public enum openingDirection { UP, DOWN, LEFT, RIGHT }
    public enum facingDirection { X, Z }

    public openingDirection direction;
    public facingDirection facing;
    public float openingTime = 0.5f;
    public state startingState;

    state currentState;
    float dirSide;
    float dirScale;
    Vector3 originalPos;

    void Start()
    {
        currentState = state.CLOSED;
        originalPos = transform.position;
        if (direction == openingDirection.UP || direction == openingDirection.DOWN)//getting the scale of which will be used to open and close the door
        {
            dirScale = transform.localScale.y;
        }
        else
        {
            dirScale = facing == facingDirection.X ? transform.localScale.z : transform.localScale.x;
        }
        dirSide = dirScale / 2;
        SetState(startingState);
    }

    public void SetState(state stateToMove)//public method to open and close the door
    {
        if (stateToMove != currentState)
        {
            currentState = stateToMove;
            switch (direction)//calling the correct method to get the proper opening direction
            {
                case openingDirection.UP:
                    StopCoroutine(Vertical(-1));
                    StartCoroutine(Vertical(1));
                    break;
                case openingDirection.DOWN:
                    StopCoroutine(Vertical(1));
                    StartCoroutine(Vertical(-1));
                    break;
                case openingDirection.LEFT:
                    StopCoroutine(Horizontal(1));
                    StartCoroutine(Horizontal(-1));
                    break;
                case openingDirection.RIGHT:
                    StopCoroutine(Horizontal(-1));
                    StartCoroutine(Horizontal(1));
                    break;
            }
        }
    }

    IEnumerator Vertical(int openingDir)//1 UP, -1 DOWN
    {
        float percent = 0;
        float openingSpeed = 1 / openingTime;

        while (percent < 1)
        {
            percent += Time.deltaTime * openingSpeed;
            float dirCorrectedPercent = Mathf.Abs((int)currentState - percent);

            transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(dirScale, 0, dirCorrectedPercent), transform.localScale.z);
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(originalPos.y, originalPos.y + dirSide * openingDir, dirCorrectedPercent), transform.position.z);
            yield return null;
        }
    }

    IEnumerator Horizontal(int openingDir)//1 RIGHT, -1 LEFT
    {
        float percent = 0;
        float openingSpeed = 1 / openingTime;

        while (percent < 1)
        {
            percent += Time.deltaTime * openingSpeed;
            float dirCorrectedPercent = Mathf.Abs((int)currentState - percent);

            if (facing == facingDirection.X)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Lerp(dirScale, 0, dirCorrectedPercent));
                transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(originalPos.z, originalPos.z + dirSide * openingDir * -1, dirCorrectedPercent));
            }
            else
            {
                transform.localScale = new Vector3(Mathf.Lerp(dirScale, 0, dirCorrectedPercent), transform.localScale.y, transform.localScale.z);
                transform.position = new Vector3(Mathf.Lerp(originalPos.x, originalPos.x + dirSide * openingDir, dirCorrectedPercent), transform.position.y, transform.position.z);
            }
            yield return null;
        }
    }
}
