using System;
using System.Collections;
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
    [Range(0, 1)]
    public float closePercent = 1;

    state currentState;
    float dirSide;
    float dirScale;
    Vector3 originalPos;
    bool isSomethingRunning;
    int openingDir;
    Action correctDirAndFace;

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

            if (isSomethingRunning) return;

            switch (direction)//calling the correct method to get the proper opening direction
            {
                case openingDirection.UP:
                    openingDir = 1;
                    correctDirAndFace = Vertical;
                    break;
                case openingDirection.DOWN:
                    openingDir = -1;
                    correctDirAndFace = Vertical;
                    break;
                case openingDirection.LEFT:
                    openingDir = -1;
                    if (facing == facingDirection.X)
                    {
                        correctDirAndFace = HorizFaceX;
                    }
                    else
                    {
                        correctDirAndFace = HorizFaceZ;
                    }
                    break;
                case openingDirection.RIGHT:
                    openingDir = 1;

                    if (facing == facingDirection.X)
                    {
                        correctDirAndFace = HorizFaceX;
                    }
                    else
                    {
                        correctDirAndFace = HorizFaceZ;
                    }
                    break;
            }
            StartCoroutine(DoorMover());
        }
    }

    void Vertical()
    {
        transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(0, dirScale, closePercent), transform.localScale.z);
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(originalPos.y + dirSide * openingDir, originalPos.y, closePercent), transform.position.z);
    }

    void HorizFaceX()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Lerp(0, dirScale, closePercent));
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(originalPos.z + dirSide * openingDir * -1, originalPos.z, closePercent));
    }

    void HorizFaceZ()
    {
        transform.localScale = new Vector3(Mathf.Lerp(0, dirScale, closePercent), transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(Mathf.Lerp(originalPos.x + dirSide * openingDir, originalPos.x, closePercent), transform.position.y, transform.position.z);
    }

    IEnumerator DoorMover()
    {
        isSomethingRunning = true;
        float openingSpeed = 1 / openingTime;

        while ((currentState == state.OPEN && closePercent > 0) || (currentState == state.CLOSED && closePercent < 1))
        {
            closePercent += Time.deltaTime * openingSpeed * (currentState == state.OPEN ? -1 : 1);

            correctDirAndFace();

            yield return null;
        }

        isSomethingRunning = false;
    }
}
