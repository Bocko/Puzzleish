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
    Transform itemT;
    Collider itemColl;
    Rigidbody itemRB;

    PlayerLook playerLook;
    InputHandler inputHandler;

    void Start()
    {
        playerLook = GetComponent<PlayerLook>();
        inputHandler = GetComponent<InputHandler>();
        HandEmpty = true;
    }

    bool fire1Down = false;
    void Update()
    {
        fire1Down = inputHandler.Fire1Down || fire1Down; //fire1Down is true if the fire1 button was pressed down or if it was previously pressed
        fire1Down = (fire1Down && inputHandler.Fire1Up) ? false : fire1Down; //reseting fireiDown if its true and the button was released

        fire1Down = (!HandEmpty && inputHandler.Fire3Up) ? false : fire1Down; //reseting fire1 if fire3 down and there is something in the hand

        //if the players hand is empty cast a ray to check if something that can be picked up is in front of it 
        if (HandEmpty)
        {
            EmptyHandRaycasting();
        }// if the player lets go of the mouse button the object falls?
        else if (!fire1Down) //something in hand and left mouse is up
        {
            Releaseitem();//item gets released and if the mouse wheel was also let go the item will get some forward force 
            if (inputHandler.Fire3Up)
            {
                ThrowHeldItem();
            }
            else
            {
                AddTossForce();
            }
            ResetItemCompHolders();

        }// move the selected object to the offset point relative to the player
        else if (fire1Down) //somehting in hand and left mouse is down
        {
            MoveItemWithWheel();
            if (inputHandler.Fire3Down)
            {
                ChargeThrow();
            }

            //if the right mouse button is held down rotate the object holder by the mouse
            if (inputHandler.Fire2Down)
            {
                ItemRotation();
            }

            itemT.SetPositionAndRotation(holdingPoint.position, holdingPoint.rotation);//applying pos and rot of holdingPoint to the held item

            Debug.DrawRay(headPivotPoint.position, (holdingPoint.position - headPivotPoint.position).normalized * distance);
        }

        ResetLookLock();
        ResetCharge();

        //debug ray for the throwforce
        Debug.DrawRay(headPivotPoint.position + headPivotPoint.forward * maxPickupDistance, GetThrowForce());
    }

    void EmptyHandRaycasting()
    {
        if (Physics.Raycast(headPivotPoint.position, headPivotPoint.forward, out hitInfo, maxPickupDistance, pickupMask, QueryTriggerInteraction.Ignore))
        {
            if (!hitInfo.collider.CompareTag(Tags.MoveableTag))//check if the object is moveable
            {
                currentItemName = "";
                moveable = false;
                return;
            }

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
    }

    void ItemRotation()
    {
        playerLook.lockMouseMovement = true;
        //rotating holding point around it self on the Y axis and on a body rotation corrected X axis so the object always rotates the same way and the body's rotation does not matter
        //or rotates around a point infront of the player at the same distance as the objects center from the players head
        //its not yet decided to which one to use

        holdingPoint.RotateAround(holdingPoint.position, Vector3.up, inputHandler.MouseX * rotateSens);
        holdingPoint.RotateAround(holdingPoint.position, Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * Vector3.right, inputHandler.MouseY * rotateSens);
    }

    void GrabItem()
    {
        HandGrabbed?.Invoke();

        HandEmpty = false;

        itemT = hitInfo.transform;
        itemColl = hitInfo.collider;
        itemRB = itemColl.attachedRigidbody;

        distance = Vector3.Distance(headPivotPoint.position, itemT.position);
        distanceBetweenGrabPointAndCenter = Vector3.Distance(itemT.position, hitInfo.point);

        holdingPoint.SetPositionAndRotation(itemT.position, itemT.rotation);

        EnablePickedupObject(false);
    }

    void MoveItemWithWheel()
    {
        float moveGrabedItem = inputHandler.MouseWheel;

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
        HandReleased?.Invoke();

        HandEmpty = true;
        holdingPoint.localPosition = Vector3.zero;
        EnablePickedupObject(true);
    }

    void ResetLookLock()
    {
        if (playerLook.lockMouseMovement && !inputHandler.Fire2Down)
        {
            playerLook.lockMouseMovement = false;
        }
    }

    void ResetCharge()
    {
        if (currentCharge > 0 && HandEmpty)
        {
            currentCharge = 0;
        }
    }

    void ResetItemCompHolders()
    {
        itemT = null;
        itemColl = null;
        itemRB = null;
    }

    void AddTossForce()
    {
        itemRB.AddForce(GetThrowForce(), ForceMode.Impulse);
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
        float finalForce = maxForce * Mathf.Clamp01(currentCharge);
        currentCharge = 0;
        itemRB.AddForce(headPivotPoint.forward * finalForce, ForceMode.Impulse);
    }

    Vector3 GetThrowForce()
    {
        //rotating the force so its always perpendicular to the players x axis
        //the force is rotated by multipling it with the main bodys y rotation
        //rotating on the x axis so when looking up or down the force is always perpendicular to the players y axis
        //the force is rotated by the headPivotpoints x axis and then its rotated by the bodys y axis
        Vector3 rotatedXDirection = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up)
                                   * Vector3.right * Mathf.Clamp(inputHandler.MouseXPlayerSens, -minMaxMouseThrowInput, minMaxMouseThrowInput);
        Vector3 rotatedYDirection = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * Quaternion.AngleAxis(headPivotPoint.eulerAngles.x, Vector3.right)
                                   * Vector3.up * Mathf.Clamp(inputHandler.MouseYPlayerSens, -minMaxMouseThrowInput, minMaxMouseThrowInput);

        return (rotatedXDirection + rotatedYDirection) * throwForce;
    }

    void EnablePickedupObject(bool enable)//now objectives needs to have 2 colliders :)))) one for collision and it gets disabled when picked up the other is just a trigger to still get the event when the it enters the objective target trigger :)
    {
        itemRB.velocity = Vector3.zero;
        itemRB.angularVelocity = Vector3.zero;
        itemRB.useGravity = enable;
        itemColl.enabled = enable;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(headPivotPoint.position, headPivotPoint.forward * maxPickupDistance);

        Vector3 dirToCurrentObjectCenter = (holdingPoint.position - headPivotPoint.position).normalized;

        Gizmos.DrawSphere(headPivotPoint.position + dirToCurrentObjectCenter * distance, .2f);
    }
}
