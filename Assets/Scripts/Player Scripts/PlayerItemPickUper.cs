using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemPickUper : MonoBehaviour
{
    public event System.Action HandGrabbed;
    public event System.Action HandReleased;

    [Header("Points")]
    public Transform headPivotPoint;
    public Transform holdingPoint;

    [Header("Hold Settings")]
    public float maxPickupDistance = 3;
    public LayerMask pickupMask;
    public float rotateSens = .75f;
    public float throwForce = 10;
    public float minHoldingDistance = 1;
    public float maxHoldingDistance = 3;
    public float minMaxMouseThrowInput = 5;

    [Header("Throw Settings")]
    public float maxCharge = 1;
    public float currentCharge = 0;
    public float chargeRate = .5f;
    public float maxForce = 20;

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

    bool fire1Down = false;
    void Update()
    {
        fire1Down = Input.GetButtonDown("Fire1") || fire1Down; //fire1Down is true if the fire1 button was pressed down or if it was previously pressed
        fire1Down = (fire1Down && Input.GetButtonUp("Fire1")) ? false : fire1Down; //reseting fireiDown if its true the button was released

        bool fire3Up = Input.GetButtonUp("Fire3");

        fire1Down = (!HandEmpty && fire3Up) ? false : fire1Down; //reseting if fire3 down and theres something in the hand

        bool fire3Down = Input.GetAxisRaw("Fire3") == 1;
        bool fire2Down = Input.GetAxisRaw("Fire2") == 1;
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //if the players hand is empty cast a ray to check if something that can be picked up is in front of it 
        if (HandEmpty)
        {
            //print("hand empty");
            if (Physics.Raycast(headPivotPoint.position, headPivotPoint.forward, out hitInfo, maxPickupDistance, pickupMask, QueryTriggerInteraction.Ignore))
            {
                if (!hitInfo.collider.CompareTag("Moveable"))//check if the object is moveable
                {
                    currentItemName = "";
                    moveable = false;
                    return;
                }

                print("hit something");
                currentItemName = hitInfo.collider.name;
                moveable = true;

                //if the player hold mouse 0 (left mouse) the object can be moved by the player
                if (fire1Down)
                {
                    GrabItem();
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
            Releaseitem();//item get released and if the mouse wheel was also let go the item will get some forward force 
            if (fire3Up)
            {
                ThrowHeldItem();
            }
            else
            {
                AddTossForce(mouseX, mouseY);
            }

        }// move the selected object to the offset point relative to the player
        else if (fire1Down) //somehting in hand and left mouse is down
        {
            print("hand holding something");
            MoveItemWithWheel();
            if (fire3Down)
            {
                ChargeThrow();
            }

            //if the right mouse button is held down rotate the object holder by the mouse
            if (fire2Down)
            {
                ItemRotation(mouseX, mouseY);
            }

            hitInfo.transform.SetPositionAndRotation(holdingPoint.position, holdingPoint.rotation);//applying pos and rot of holdingPoint to the held item

            Debug.DrawRay(headPivotPoint.position, (holdingPoint.position - headPivotPoint.position).normalized * distance);
        }

        if (playerLook.lockMouseMovement && !fire2Down)
        {
            playerLook.lockMouseMovement = false;
        }

        if (currentCharge > 0 && HandEmpty)
        {
            currentCharge = 0;
        }

        //debug ray for the throwforce
        Debug.DrawRay(headPivotPoint.position + headPivotPoint.forward * maxPickupDistance, GetThrowForce(mouseX, mouseY));
    }

    void ItemRotation(float mouseX, float mouseY)
    {
        playerLook.lockMouseMovement = true;
        //rotating holding point around it self on the Y axis and on a body rotation corrected X axis so the object always rotates the same way and the body's rotation does not matter
        //or rotates around a point infront of the player at the same distance as the objects center from the players head
        //its not yet decided to which one to use

        holdingPoint.RotateAround(holdingPoint.position, Vector3.up, mouseX * rotateSens);
        holdingPoint.RotateAround(holdingPoint.position, Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * Vector3.right, mouseY * rotateSens);/*
        holdingPoint.RotateAround(headPivotPoint.position + headPivotPoint.forward * distance, Vector3.up, mouseX * rotateSens * Time.deltaTime);
        holdingPoint.RotateAround(headPivotPoint.position + headPivotPoint.forward * distance, Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * Vector3.right, mouseY * rotateSens * Time.deltaTime);*/
    }

    void GrabItem()
    {
        print("grabbed something");

        HandGrabbed?.Invoke();

        HandEmpty = false;

        distance = Vector3.Distance(headPivotPoint.position, hitInfo.transform.position);
        distanceBetweenGrabPointAndCenter = Vector3.Distance(hitInfo.transform.position, hitInfo.point);

        holdingPoint.SetPositionAndRotation(hitInfo.transform.position, hitInfo.transform.rotation);

        EnablePickedupObject(false);
    }

    void MoveItemWithWheel()
    {
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
    }

    void Releaseitem()
    {
        print("hand let go");

        HandReleased?.Invoke();

        HandEmpty = true;
        holdingPoint.position = headPivotPoint.position;
        EnablePickedupObject(true);
    }

    void AddTossForce(float mouseX, float mouseY)
    {
        print("toss");
        hitInfo.collider.attachedRigidbody.AddForce(GetThrowForce(mouseX, mouseY), ForceMode.Impulse);
    }

    void ChargeThrow()
    {
        if (currentCharge < maxCharge)
        {
            currentCharge += Time.deltaTime * chargeRate;
        }
    }

    void ThrowHeldItem()
    {
        print("throwing");
        float finalForce = maxForce * Mathf.Clamp01(currentCharge);
        currentCharge = 0;
        hitInfo.collider.attachedRigidbody.AddForce(headPivotPoint.forward * finalForce, ForceMode.Impulse);
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
        return mouse * playerLook.mouseSens;
    }

    void EnablePickedupObject(bool enable)//now objectives needs to have 2 colliders :)))) one for collision and it gets disabled when picked up the other is just a trigger to still get the event when the it enters the objective target trigger :)
    {
        hitInfo.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        hitInfo.transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        hitInfo.transform.GetComponent<Rigidbody>().useGravity = enable;
        hitInfo.collider.enabled = enable;
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(headPivotPoint.position, headPivotPoint.forward * maxPickupDistance);

        Vector3 dirToCurrentObjectCenter = (holdingPoint.position - headPivotPoint.position).normalized;

        Gizmos.DrawSphere(headPivotPoint.position + dirToCurrentObjectCenter * distance, .2f);
    }
}
