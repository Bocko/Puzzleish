using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementRigidbody : MonoBehaviour
{
    [Header("Ground Check")]
    public Transform groundChecker;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Movement Settings")]
    public float speed = 7f;
    public float AltMaxSpeed = 3f;
    public float MaxSpeed = 3f;
    public float jumpForce = 3f;

    Rigidbody playerRB;
    Vector3 moveVelocity;
    bool onGround;
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        onGround = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);

        float movementVertical = Input.GetAxisRaw("Vertical");
        float movementHorizontal = Input.GetAxisRaw("Horizontal");

        Vector3 inputDir = (movementHorizontal * transform.right + movementVertical * transform.forward).normalized;
        moveVelocity = inputDir * speed;

        if (Input.GetButtonDown("Jump") && onGround)
        {
            moveVelocity.y += jumpForce;
        }
    }
    void FixedUpdate()
    {
        if (playerRB.velocity.magnitude < (Input.GetAxis("Walk") == 0 ? MaxSpeed : AltMaxSpeed))
        {
            playerRB.AddForce(moveVelocity * Time.deltaTime, ForceMode.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundChecker.position, groundDistance);
        Gizmos.DrawRay(transform.position, moveVelocity);
    }
}
