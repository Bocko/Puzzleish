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
    public float walkSpeed = 3f;
    public float inAirSpeed = 3f;
    public float mass = 10;
    public float gravity = 10f;
    public float gravityMultiplier = 1f;
    public float jumpHeight = 3f;
    public float maxOppositeAngleCutOff = 130;

    [Header("Crouch Settings")]
    public Transform headChecker;
    public float headCheckerDistance = .1f;
    public LayerMask headMask;
    public float defaultHeight = 2;
    public float crouchedHeight = 1.5f;
    public float crouchTime = .01f;
    float verticalAdjusmentAmount = .25f;

    Vector3 velocity;
    Vector3 moveVelocity;

    CharacterController controller;
    PlayerLook playerLook;
    Transform playerBody;

    bool onGround
    {
        get
        {
            return controller.isGrounded;
        }
    }
    bool isCrouched = false;
    bool walking = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerLook = GetComponent<PlayerLook>();
        playerBody = transform.Find("Player Body");
        verticalAdjusmentAmount = defaultHeight - crouchedHeight;
    }

    void Update()
    {
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
        bool crouchDown = Input.GetAxis("Crouch") == 1;
        bool walkDown = Input.GetAxis("Walk") == 1;

        //normalized input vector
        Vector3 inputDir = (movementHorizontal * transform.right + movementVertical * transform.forward).normalized;

        if (isCrouched != crouchDown)
        {
            Crouch();
        }

        if (onGround)
        {
            //if on the ground and the walk key is held down reduce the speed
            moveVelocity = inputDir * ((walking || walkDown) ? walkSpeed : speed);
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
            //saving the horizontal velocity so the jump keeps the players "momentum"(?)
            velocity += moveVelocity;

        }

        moveVelocity += velocity;
    }
    void FixedUpdate()
    {
        FakeDownForceBelowPlayer();
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

    void FakeDownForceBelowPlayer()
    {
        //adding fake force downwards on obejct that is below the player
        if (Physics.Raycast(groundChecker.position, Vector3.down, out RaycastHit hitInfo, groundDistance, groundMask))
        {
            testSphere.position = hitInfo.point;
            if (hitInfo.collider.attachedRigidbody != null)
            {
                hitInfo.collider.attachedRigidbody.AddForceAtPosition(mass * Mathf.Abs(velocity.y) * Vector3.down, hitInfo.point);
            }
        }
    }

    void Crouch()
    {
        //player can crouch when its not already crouched
        //when crouching the player get lowered
        if (!isCrouched)
        {
            controller.height = crouchedHeight;
            transform.Translate(0, -verticalAdjusmentAmount / 2, 0);
            playerLook.AdjustCamAndHeadPivot(-verticalAdjusmentAmount / 2);
            playerBody.localScale = new Vector3(1, crouchedHeight / defaultHeight, 1);
            isCrouched = true;
            walking = true;
        }
        else
        {
            if (CheckAboveForUncrouch())
            {
                controller.height = defaultHeight;
                transform.Translate(0, verticalAdjusmentAmount / 2, 0);
                playerBody.localScale = Vector3.one;
                playerLook.AdjustCamAndHeadPivot(verticalAdjusmentAmount / 2);
                isCrouched = false;
                walking = false;
            }
        }
    }

    bool CheckAboveForUncrouch()
    {
        return !Physics.CheckSphere(headChecker.position + Vector3.up * verticalAdjusmentAmount, headCheckerDistance, headMask);
    }

    IEnumerator AnimateCrouch()
    {
        float percent = 0;
        float crouchSpeed = 1f / crouchTime;
        print(1f / crouchTime);
        while (percent < 1)
        {
            percent += Time.deltaTime * crouchSpeed;

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundChecker.position, groundDistance);
        Gizmos.DrawSphere(headChecker.position + Vector3.up * verticalAdjusmentAmount, headCheckerDistance);
    }

    private void OnDisable()
    {
        velocity = Vector3.zero;
    }
}
