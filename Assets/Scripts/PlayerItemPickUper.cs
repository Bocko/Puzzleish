using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemPickUper : MonoBehaviour
{
    public Transform headPivotPoint;
    public Transform holdingPoint;
    public float maxPickupDistance = 3;
    public LayerMask pickupMask;
    public float rotateSens = 75;
    public float throwForce = 10;
    public float minHoldingDistance = 1;
    public float maxHoldingDistance = 3;
    public float minMaxMouseThrowInput = 5;

    public string currentItemName;
    public bool moveable;

    bool HandEmpty;
    float distance;
    float distanceBetweenGrabPointAndCenter;

    Ray ray;
    RaycastHit hitInfo;

    PlayerLook playerLook;
    void Start()
    {
        playerLook = GetComponent<PlayerLook>();
        ray = new Ray();
        HandEmpty = true;
    }

    void Update()
    {
        float mouse0Pos = Input.GetAxisRaw("Fire1");
        float mouse1Pos = Input.GetAxisRaw("Fire2");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        //these two should only be in the mouse let go branch but the debug ray uses it...
        float mouseXPlayerSens = mouseX * playerLook.mouseSens * Time.deltaTime;
        float mouseYPlayerSens = mouseY * playerLook.mouseSens * Time.deltaTime;

        //if the players hand is empty cast a ray to check if something that can be picked up is in front of it 
        if (HandEmpty)
        {
            print("hand empty");
            ray.origin = headPivotPoint.position;
            ray.direction = headPivotPoint.forward;
            if (Physics.Raycast(ray, out hitInfo, maxPickupDistance, pickupMask, QueryTriggerInteraction.Ignore))
            {
                print("hit something");
                currentItemName = hitInfo.collider.name;
                moveable = true;
                //if the player hold mouse 0 (left mouse) the object can be moved by the player
                if (mouse0Pos == 1)
                {
                    print("grabbed something");
                    HandEmpty = false;

                    distance = Vector3.Distance(headPivotPoint.position, hitInfo.transform.position);
                    distanceBetweenGrabPointAndCenter = Vector3.Distance(hitInfo.transform.position, hitInfo.point);

                    holdingPoint.SetPositionAndRotation(hitInfo.transform.position, hitInfo.transform.rotation);

                    hitInfo.collider.attachedRigidbody.velocity = Vector3.zero;
                    hitInfo.collider.attachedRigidbody.angularVelocity = Vector3.zero;

                    EnablePickedupObject(false);
                }
            }
            else
            {
                currentItemName = "";
                moveable = false;
            }

        }// if the player lets go of the mouse button the object falls?
        else if (mouse0Pos == 0) //something in hand and left mouse is up
        {
            print("hand let go");

            HandEmpty = true;
            holdingPoint.position = headPivotPoint.position;
            EnablePickedupObject(true);

            //rotating the force so its always perpendicular to the player
            //the force is rotated by multipling it with the main bodys rotation
            Vector3 rotatedDirection = Quaternion.AngleAxis(headPivotPoint.parent.parent.rotation.eulerAngles.y, Vector3.up) 
                                       * new Vector3(Mathf.Clamp(mouseXPlayerSens, -minMaxMouseThrowInput, minMaxMouseThrowInput), Mathf.Clamp(mouseYPlayerSens, -minMaxMouseThrowInput, minMaxMouseThrowInput), 0);
            hitInfo.collider.attachedRigidbody.AddForce(rotatedDirection * throwForce, ForceMode.Impulse);

        }// move the selected object to the offset point relative to the player
        else if (mouse0Pos == 1) //somehting in hand and left mouse is down
        {
            print("hand holding something");

            float moveGrabedItem = Input.GetAxisRaw("Mouse ScrollWheel");

            //if theres something in the hand and there was scrolling adjust the distance accordingly
            Vector3 dirToCurrentObjectCenter = (holdingPoint.position - headPivotPoint.position).normalized;
            if (moveGrabedItem != 0)
            {
                float updatedDistance = distance + moveGrabedItem;
                if (updatedDistance <= maxHoldingDistance + distanceBetweenGrabPointAndCenter && updatedDistance >= minHoldingDistance - distanceBetweenGrabPointAndCenter)
                {
                    distance = updatedDistance;
                    holdingPoint.position = headPivotPoint.position + dirToCurrentObjectCenter * distance;
                }
            }

            //if the right mouse button is held down rotate the object holder by the mouse
            if (mouse1Pos == 1)
            {
                //add to the rotation
                holdingPoint.rotation = Quaternion.Euler(holdingPoint.rotation.eulerAngles + mouseX * rotateSens * Time.deltaTime * Vector3.up);
                holdingPoint.rotation = Quaternion.Euler(holdingPoint.rotation.eulerAngles + mouseY * rotateSens * Time.deltaTime * Vector3.forward);
            }

            hitInfo.transform.SetPositionAndRotation(holdingPoint.position, holdingPoint.rotation);

            Debug.DrawRay(headPivotPoint.position, dirToCurrentObjectCenter * distance);
        }

        //debug ray for the throwforce
        Vector3 rotatedDirection2 = Quaternion.AngleAxis(headPivotPoint.parent.parent.rotation.eulerAngles.y, Vector3.up) 
                                    * new Vector3(Mathf.Clamp(mouseXPlayerSens, -minMaxMouseThrowInput, minMaxMouseThrowInput), Mathf.Clamp(mouseYPlayerSens, -minMaxMouseThrowInput, minMaxMouseThrowInput), 0);
        Debug.DrawRay(headPivotPoint.position + headPivotPoint.forward * maxPickupDistance, rotatedDirection2 * throwForce);
    }

    void EnablePickedupObject(bool enable)
    {
        if (enable)
        {
            hitInfo.collider.enabled = enable;
            hitInfo.collider.attachedRigidbody.useGravity = enable;
        }
        else
        {
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
