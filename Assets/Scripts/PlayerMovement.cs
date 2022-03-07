using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Transform testSphere;

    [Header("Ground Check")]
    public Transform groundChecker;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Movement Settings")]
    public float speed = 7f;
    public float AltSpeed = 3f;
    public float inAirSpeed = 3f;
    public float mass = 10;
    public float gravity = 10f;
    public float gravityMultiplier = 1f;
    public float jumpHeight = 3f;
    public float maxOppositeAngleCutOff = 130;

    CharacterController controller;
    Vector3 velocity;
    Vector3 moveVelocity;
    bool onGround
    {
        get
        {
            return controller.isGrounded;
        }
    }
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        //adding fake force downwards on obejct that is below the player
        Ray ray = new Ray(groundChecker.position, new Vector3(0, -1f, 0));
        if (Physics.Raycast(ray, out RaycastHit hitInfo, groundDistance, groundMask))
        {
            testSphere.position = hitInfo.point;
            if (hitInfo.collider.attachedRigidbody != null)
            {
                hitInfo.collider.attachedRigidbody.AddForceAtPosition(mass * Mathf.Abs(velocity.y) * Vector3.down, hitInfo.point);
            }
        }

        //adding gravity over time
        velocity.y += -gravity * gravityMultiplier * Time.deltaTime;

        if (onGround && velocity.y < 0)
        {
            //if on the ground reset the saved velocity to 0 and add const gravity
            velocity = Vector3.zero;
            velocity.y = -gravity;
        }

        float movementVertical = Input.GetAxisRaw("Vertical");
        float movementHorizontal = Input.GetAxisRaw("Horizontal");

        //normalized input vector
        Vector3 inputDir = (movementHorizontal * transform.right + movementVertical * transform.forward).normalized;

        if (onGround)
        {
            //if on the ground and the walk key is held down reduce the speed
            moveVelocity = inputDir * (Input.GetAxis("Walk") == 0 ? speed : AltSpeed);
        }
        else
        {
            //if not on the ground use inAirSpeed insted to give a minimal air control
            moveVelocity = inputDir * inAirSpeed;

            //if the angle between the saved velocity and the current input vector is higher than the max zero the saved velocity
            //note: this is only for the horizontal plane
            if (Vector2.Angle(new Vector2(velocity.x, velocity.z), new Vector2(moveVelocity.x, moveVelocity.z)) > maxOppositeAngleCutOff)
            {
                velocity.x = 0;
                velocity.z = 0;
            }
        }
        //if the jump key is pressed and the player is on the ground add enough upwards velocity to reach the set jump height
        if (Input.GetButtonDown("Jump") && onGround)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2 * gravity * gravityMultiplier);
            velocity += moveVelocity;

        }

        moveVelocity += velocity;
    }
    void FixedUpdate()
    {
        //limit speed so that when in the air you cant go faster than the limit when holding the forward key
        float tempY = moveVelocity.y;
        moveVelocity.y = 0;
        if (moveVelocity.sqrMagnitude > Mathf.Pow(speed, 2))
        {
            moveVelocity = moveVelocity.normalized * speed;
        }
        moveVelocity.y = tempY;// = new Vector3(moveVelocity.x, tempY, moveVelocity.z);

        controller.Move(moveVelocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundChecker.position, groundDistance);
    }

    private void OnDisable()
    {
        velocity = Vector3.zero;
    }
}
