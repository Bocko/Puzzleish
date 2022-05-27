using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
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
    public float sqrWallBlockingVelocityLimit = 5;

    [Header("Crouch Settings")]
    public Transform headChecker;
    public float headCheckerDistance = .1f;
    public LayerMask headMask;
    public float defaultHeight = 2;
    public float crouchedHeight = 1.5f;
    public float crouchTime = 0.1f;
    public Transform playerBody;

    float verticalAdjusmentAmount = .25f;

    Vector3 jetpackVelocity; // velocity for the jetpack
    Vector3 verticalVelocity; // vertical velocity for appling gravity and to add force for jump
    Vector3 savedVelocity; // saved velocity to saved the horizontal velocity before jump to keep the "momentum" in the air
    Vector3 moveVelocity; // move velocity is to move around with the controls
    Vector3 finalVelocity; // final velocity is to add all the above velocities together

    CharacterController controller;
    PlayerLook playerLook;
    InputHandler inputHandler;

    public bool onGround
    {
        get
        {
            return controller.isGrounded;
        }
        private set { }
    }
    public bool isCrouched { get; private set; }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerLook = GetComponent<PlayerLook>();
        inputHandler = GetComponent<InputHandler>();
        verticalAdjusmentAmount = (defaultHeight - crouchedHeight) / 2;
    }

    void Update()
    {
        Gravity();

        //normalized input vector
        Vector3 inputDir = (inputHandler.Horizontal * transform.right + inputHandler.Vertical * transform.forward).normalized;

        if (isCrouched != inputHandler.Crouch)
        {
            Crouch();
        }

        MovementCalculation(inputDir);

        //if the jump key is pressed and the player is on the ground add enough upwards velocity to reach the set jump height
        if (inputHandler.Jump && onGround)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        finalVelocity = Vector3.zero;

        FakeDownForceBelowPlayer();

        moveVelocity += savedVelocity;

        SpeedLimiter();

        verticalVelocity += jetpackVelocity;

        finalVelocity += verticalVelocity;
        finalVelocity += moveVelocity;

        VelocityLimitForStuckage();

        controller.Move(finalVelocity * Time.deltaTime);
    }

    void Gravity()
    {
        //adding gravity over time
        verticalVelocity.y += -gravity * gravityMultiplier * Time.deltaTime;

        if (onGround && verticalVelocity.y < 0)
        {
            //if on the ground reset the saved velocity to 0 and add const gravity to vertical velocity
            savedVelocity = Vector3.zero;
            verticalVelocity.y = -gravity;
        }
    }

    void MovementCalculation(Vector3 inputDir)
    {
        if (onGround)
        {
            //if on the ground and the walk key is held down or the player is crouched reduce movement speed
            moveVelocity = inputDir * ((isCrouched || inputHandler.Walk) ? walkSpeed : speed);
        }
        else
        {
            //if not on the ground use inAirSpeed insted to give a minimal air control
            moveVelocity = inputDir * inAirSpeed;

            //if the angle between the saved velocity and the current input vector is higher than the max reset the saved velocity
            //note: this is only for the horizontal velocity
            if (Vector2.Angle(new Vector2(savedVelocity.x, savedVelocity.z), new Vector2(moveVelocity.x, moveVelocity.z)) > maxOppositeAngleCutOff)
            {
                savedVelocity = Vector3.zero;
            }
        }
    }

    void Jump()
    {
        verticalVelocity.y = Mathf.Sqrt(jumpHeight * 2 * gravity * gravityMultiplier);
        //saving the horizontal velocity so the jump keeps the players "momentum"(?)
        savedVelocity = controller.velocity;
    }

    void SpeedLimiter()
    {
        //limit speed so that when in the air you cant go faster than the limit when holding the forward key
        if (moveVelocity.sqrMagnitude > Mathf.Pow(speed, 2))
        {
            moveVelocity = moveVelocity.normalized * speed;
        }
    }

    void FakeDownForceBelowPlayer()
    {
        //adding fake force downwards on obejct that is below the player
        if (Physics.Raycast(groundChecker.position, Vector3.down, out RaycastHit hitInfo, groundDistance, groundMask))
        {
            Rigidbody itemRB = hitInfo.collider.attachedRigidbody;
            if (itemRB == null) return;
            itemRB.AddForceAtPosition(mass * Mathf.Abs(verticalVelocity.y) * Vector3.down, hitInfo.point);
        }
    }

    void VelocityLimitForStuckage()
    {
        //setting player speed to actual speed by the character controller when the inputs magnitude is bigger than 0 and when the actual speed is lower then the set limit
        if (moveVelocity.sqrMagnitude > 0 && new Vector3(controller.velocity.x, 0, controller.velocity.z).sqrMagnitude < sqrWallBlockingVelocityLimit)
        {
            savedVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        }
        //setting vertical speed to 0 when the players vertical velocity is higher than 0 and when the the actual velocity is 0 and when the jetpack velocity is 0 and when the players not on the ground
        if (verticalVelocity.y > 0 && controller.velocity.y == 0 && jetpackVelocity.y == 0 && !onGround)
        {
            verticalVelocity = Vector3.zero;
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
            StartCoroutine(AnimateCrouch(0));
            isCrouched = true;
        }
        else
        {
            if (CheckAboveForUncrouch())
            {
                StopCoroutine(AnimateCrouch(0));
                StartCoroutine(AnimateCrouch(1));
                isCrouched = false;
            }
        }
    }

    bool CheckAboveForUncrouch()// check if theres something above the palyers head when crouched
    {
        return !Physics.CheckSphere(headChecker.position + verticalAdjusmentAmount * Vector3.up, headCheckerDistance, headMask);
    }

    IEnumerator AnimateCrouch(int dir)//0 down, 1 up
    {
        float percent = 0;
        float crouchSpeed = 1f / crouchTime;
        while (percent < 1)
        {
            percent += Time.deltaTime * crouchSpeed;

            //if dir is 1 the percent will go bakcwards so the animation will play is reverse
            //if dir is 0 it will count like normal because it get its abs value so the anim will play like normal
            float dirCorrectedPercent = Mathf.Abs(dir - percent);

            controller.height = Mathf.Lerp(defaultHeight, crouchedHeight, dirCorrectedPercent);
            playerBody.localScale = new Vector3(1, Mathf.Lerp(1, crouchedHeight / defaultHeight, dirCorrectedPercent), 1);
            playerLook.SetCamAndHeadPivotLocalYPos(Mathf.Lerp(playerLook.camHeightInPlayer, playerLook.camHeightInPlayer - verticalAdjusmentAmount, dirCorrectedPercent));

            yield return null;
        }
    }

    public void SetJetpackVelocity(Vector3 velocity)
    {
        jetpackVelocity = velocity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundChecker.position, groundDistance);//groundchecker
        Gizmos.DrawSphere(headChecker.position + verticalAdjusmentAmount * Vector3.up, headCheckerDistance);//headchecker
    }

    private void OnDisable()
    {
        jetpackVelocity = Vector3.zero;
        verticalVelocity = Vector3.zero;
        savedVelocity = Vector3.zero;
        moveVelocity = Vector3.zero;
        finalVelocity = Vector3.zero;
    }
}
