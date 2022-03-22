using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemPickUper : MonoBehaviour
{
    public static event System.Action HandGrabbed;
    public static event System.Action HandReleased;

    public Transform headPivotPoint;
    public Transform holdingPoint;
    public float maxPickupDistance = 3;
    public LayerMask pickupMask;
    public float rotateSens = 75;
    public float throwForce = 10;
    public float minHoldingDistance = 1;
    public float maxHoldingDistance = 3;
    public float minMaxMouseThrowInput = 5;

    public string currentItemName { get; private set; }
    public bool moveable { get; private set; }

    bool HandEmpty;
    float distance;
    float distanceBetweenGrabPointAndCenter;

    RaycastHit hitInfo;

    PlayerLook playerLook;
    void Start()
    {
        playerLook = GetComponent<PlayerLook>();
        HandEmpty = true;
    }

    void Update()
    {
        bool fire1Down = Input.GetAxisRaw("Fire1") == 1;
        bool fire2Down = Input.GetAxisRaw("Fire2") == 1;
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //if the players hand is empty cast a ray to check if something that can be picked up is in front of it 
        if (HandEmpty)
        {
            //print("hand empty");
            if (Physics.Raycast(headPivotPoint.position, headPivotPoint.forward, out hitInfo, maxPickupDistance, pickupMask, QueryTriggerInteraction.Ignore))
            {
                print("hit something");
                currentItemName = hitInfo.collider.name;
                moveable = true;
                //if the player hold mouse 0 (left mouse) the object can be moved by the player
                if (fire1Down)
                {
                    print("grabbed something");

                    HandGrabbed?.Invoke();

                    HandEmpty = false;

                    distance = Vector3.Distance(headPivotPoint.position, hitInfo.transform.position);
                    distanceBetweenGrabPointAndCenter = Vector3.Distance(hitInfo.transform.position, hitInfo.point);

                    holdingPoint.SetPositionAndRotation(hitInfo.transform.position, hitInfo.transform.rotation);

                    EnablePickedupObject(false);
                }
            }
            else
            {
                currentItemName = "";
                moveable = false;
            }

        }// if the player lets go of the mouse button the object falls?
        else if (!fire1Down) //something in hand and left mouse is up
        {
            print("hand let go");

            HandReleased?.Invoke();

            HandEmpty = true;
            holdingPoint.position = headPivotPoint.position;
            EnablePickedupObject(true);

            hitInfo.collider.attachedRigidbody.AddForce(GetThrowForce(mouseX, mouseY), ForceMode.Impulse);

        }// move the selected object to the offset point relative to the player
        else if (fire1Down) //somehting in hand and left mouse is down
        {
            print("hand holding something");

            float moveGrabedItem = Input.GetAxisRaw("Mouse ScrollWheel");

            //if theres something in the hand and there was scrolling adjust the distance accordingly
            Vector3 dirToCurrentObjectCenter = (holdingPoint.position - headPivotPoint.position).normalized;
            if (moveGrabedItem != 0)
            {
                float updatedDistance = distance + moveGrabedItem;
                //adding and subtracting the difference between the ray hit point and the objects center so the scrolling wont get stuck if the object center is too far or close
                if (updatedDistance <= maxHoldingDistance + distanceBetweenGrabPointAndCenter && updatedDistance >= minHoldingDistance - distanceBetweenGrabPointAndCenter)
                {
                    distance = updatedDistance;
                    holdingPoint.position = headPivotPoint.position + dirToCurrentObjectCenter * distance;
                }
            }

            //if the right mouse button is held down rotate the object holder by the mouse
            if (fire2Down)
            {
                playerLook.lockMouseMovement = true;
                //add to the rotation
                holdingPoint.rotation = Quaternion.Euler(holdingPoint.rotation.eulerAngles + mouseX * rotateSens * Time.deltaTime * Vector3.up);
                holdingPoint.rotation = Quaternion.Euler(holdingPoint.rotation.eulerAngles + mouseY * rotateSens * Time.deltaTime * Vector3.right);
            }


            hitInfo.transform.SetPositionAndRotation(holdingPoint.position, holdingPoint.rotation);

            Debug.DrawRay(headPivotPoint.position, dirToCurrentObjectCenter * distance);
        }

        if (playerLook.lockMouseMovement && !fire2Down)
        {
            playerLook.lockMouseMovement = false;
        }

        //debug ray for the throwforce
        Debug.DrawRay(headPivotPoint.position + headPivotPoint.forward * maxPickupDistance, GetThrowForce(mouseX, mouseY));
    }

    Vector3 GetThrowForce(float mouseX, float mouseY)
    {
        //rotating the force so its always perpendicular to the players x axis
        //the force is rotated by multipling it with the main bodys y rotation
        //rotating on the x axis so when looking up or down the force is always perpendicular to the players y axis
        //the force is rotated by the headPivotpoints x axis and then its rotated by the bodys y axis
        Vector3 rotatedXDirection = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up)
                                   * Vector3.right * Mathf.Clamp(GetPlayerSensMouse(mouseX), -minMaxMouseThrowInput, minMaxMouseThrowInput);
        Vector3 rotatedYDirection = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * Quaternion.AngleAxis(headPivotPoint.eulerAngles.x, Vector3.right)
                                   * Vector3.up * Mathf.Clamp(GetPlayerSensMouse(mouseY), -minMaxMouseThrowInput, minMaxMouseThrowInput);

        return (rotatedXDirection + rotatedYDirection) * throwForce;
    }

    float GetPlayerSensMouse(float mouse)
    {
        return mouse * Time.deltaTime * playerLook.mouseSens;
    }

    void EnablePickedupObject(bool enable)//now objectives needs to have 2 colliders :)))) one for collision and it gets disabled when picked up the other is just a trigger to still get the event when the it enters the objective target trigger :)
    {
        if (enable)
        {
            hitInfo.collider.enabled = enable;
            hitInfo.collider.attachedRigidbody.isKinematic = !enable;
            hitInfo.collider.attachedRigidbody.useGravity = enable;
        }
        else
        {
            hitInfo.collider.attachedRigidbody.isKinematic = !enable;
            hitInfo.collider.attachedRigidbody.useGravity = enable;
            hitInfo.collider.enabled = enable;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(headPivotPoint.position, headPivotPoint.forward * maxPickupDistance);

        Vector3 dirToCurrentObjectCenter = (holdingPoint.position - headPivotPoint.position).normalized;

        Gizmos.DrawSphere(headPivotPoint.position + dirToCurrentObjectCenter * distance, .2f);
    }
}
