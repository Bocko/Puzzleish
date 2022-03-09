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
    public float crouchTime = 0.1f;
    float verticalAdjusmentAmount = .25f;

    Vector3 velocity;
    Vector3 moveVelocity;

    CharacterController controller;
    PlayerLook playerLook;
    Transform playerBody;

    private bool onGround
    {
        get
        {
            return controller.isGrounded;
        }
    }
    public bool isCrouched { get; private set; }
    bool walking = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerLook = GetComponent<PlayerLook>();
        playerBody = transform.Find("Player Body");
        verticalAdjusmentAmount = (defaultHeight - crouchedHeight) / 2;
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
        //when crouching the player gets lowered by half of the amount its height got reduced because its pivot in the middle so it gets smaller from the top and the bottom aswell NOTE: NOT ANYMORE WITH THE ANIMATION IN PLACE
        //Cameras and the head pivot point are lower aswell by half of the difference between the crouching and the standing height
        //bodys y scale is lowered by the ratio between the crouching and the standing height
        if (!isCrouched)
        {
            StopCoroutine(AnimateCrouch(1));
            StartCoroutine(AnimateCrouch(-1));
            //controller.height = crouchedHeight;
            //transform.Translate(0, -verticalAdjusmentAmount, 0);
            //playerLook.SetCamAndHeadPivotLocalYPos(playerLook.camHeightInPlayer - verticalAdjusmentAmount);
            //playerBody.localScale = new Vector3(1, crouchedHeight / defaultHeight, 1);
            isCrouched = true;
            walking = true;
        }
        else
        {
            if (CheckAboveForUncrouch())
            {
                StopCoroutine(AnimateCrouch(-1));
                StartCoroutine(AnimateCrouch(1));
                //controller.height = defaultHeight;
                //transform.Translate(0, verticalAdjusmentAmount, 0);
                //playerLook.SetCamAndHeadPivotLocalYPos(playerLook.camHeightInPlayer);
                //playerBody.localScale = Vector3.one;
                isCrouched = false;
                walking = false;
            }
        }
    }

    bool CheckAboveForUncrouch()
    {
        return !Physics.CheckSphere(headChecker.position + 2 * verticalAdjusmentAmount * Vector3.up, headCheckerDistance, headMask);
    }

    IEnumerator AnimateCrouch(int dir)
    {
        float percent = 0;
        float crouchSpeed = 1f / crouchTime;
        while (percent < 1)
        {
            percent += Time.deltaTime * crouchSpeed;

            if (dir == 1)
            {
                controller.height = Mathf.Lerp(crouchedHeight, defaultHeight, percent);
                playerBody.localScale = new Vector3(1, Mathf.Lerp(crouchedHeight / defaultHeight, 1, percent), 1);
                playerLook.SetCamAndHeadPivotLocalYPos(Mathf.Lerp(playerLook.camHeightInPlayer - verticalAdjusmentAmount, playerLook.camHeightInPlayer, percent));
            }
            else
            {
                controller.height = Mathf.Lerp(defaultHeight, crouchedHeight, percent);
                playerBody.localScale = new Vector3(1, Mathf.Lerp(1, crouchedHeight / defaultHeight, percent), 1);
                playerLook.SetCamAndHeadPivotLocalYPos(Mathf.Lerp(playerLook.camHeightInPlayer, playerLook.camHeightInPlayer - verticalAdjusmentAmount, percent));
            }

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundChecker.position, groundDistance);
        Gizmos.DrawSphere(headChecker.position + 2 * verticalAdjusmentAmount * Vector3.up, headCheckerDistance);
    }

    private void OnDisable()
    {
        velocity = Vector3.zero;
    }
}
