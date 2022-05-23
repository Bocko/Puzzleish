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
                    StartCoroutine(Vertical2(1));
                    break;
                case openingDirection.DOWN:
                    StartCoroutine(Vertical2(-1));
                    break;
                case openingDirection.LEFT:
                    if (facing == facingDirection.X)
                    {
                        StartCoroutine(HorizontalFaceX(-1));
                    }
                    else
                    {
                        StartCoroutine(HorizontalFaceZ(-1));
                    }
                    break;
                case openingDirection.RIGHT:
                    if (facing == facingDirection.X)
                    {
                        StartCoroutine(HorizontalFaceX(1));
                    }
                    else
                    {
                        StartCoroutine(HorizontalFaceZ(1));
                    }
                    break;
            }

            /*
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
            */
        }
    }

    IEnumerator Vertical2(int openingDir)//1 UP, -1 DOWN
    {
        isSomethingRunning = true;
        float openingSpeed = 1 / openingTime;

        while ((currentState == state.OPEN && closePercent > 0) || (currentState == state.CLOSED && closePercent < 1))
        {
            closePercent += Time.deltaTime * openingSpeed * (currentState == state.OPEN ? -1 : 1);

            transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(0, dirScale, closePercent), transform.localScale.z);
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(originalPos.y + dirSide * openingDir, originalPos.y, closePercent), transform.position.z);

            yield return null;
        }

        isSomethingRunning = false;
    }

    IEnumerator HorizontalFaceX(int openingDir)//1 RIGHT, -1 LEFT
    {
        isSomethingRunning = true;
        float openingSpeed = 1 / openingTime;

        while ((currentState == state.OPEN && closePercent > 0) || (currentState == state.CLOSED && closePercent < 1))
        {
            closePercent += Time.deltaTime * openingSpeed * (currentState == state.OPEN ? -1 : 1);

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Lerp(0, dirScale, closePercent));
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(originalPos.z + dirSide * openingDir * -1, originalPos.z, closePercent));

            yield return null;
        }
        isSomethingRunning = false;
    }

    IEnumerator HorizontalFaceZ(int openingDir)//1 RIGHT, -1 LEFT
    {
        isSomethingRunning = true;
        float openingSpeed = 1 / openingTime;

        while ((currentState == state.OPEN && closePercent > 0) || (currentState == state.CLOSED && closePercent < 1))
        {
            closePercent += Time.deltaTime * openingSpeed * (currentState == state.OPEN ? -1 : 1);

            transform.localScale = new Vector3(Mathf.Lerp(0, dirScale, closePercent), transform.localScale.y, transform.localScale.z);
            transform.position = new Vector3(Mathf.Lerp(originalPos.x + dirSide * openingDir, originalPos.x, closePercent), transform.position.y, transform.position.z);

            yield return null;
        }
        isSomethingRunning = false;
    }
    /*
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
    */
}
