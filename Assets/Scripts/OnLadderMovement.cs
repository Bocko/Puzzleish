using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLadderMovement : MonoBehaviour
{
    public Transform headPivotPoint;
    public float onLadderSpeed = 7f;
    public float onLadderJumpForce = 100f;
    public string ladderTag = "Ladder";

    bool onLadder = false;
    PlayerMovement playerMovement;
    PlayerJetpack playerJetpack;
    CharacterController controller;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerJetpack = GetComponent<PlayerJetpack>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        //on ladder is triggered when the player is in a trigger marked with "Ladder" tag
        //when on ladder the default player movement is disabled and this script moves the player
        if (onLadder)
        {
            float movementVertical = Input.GetAxisRaw("Vertical");
            float movementHorizontal = Input.GetAxisRaw("Horizontal");

            //instead of moving forward of the body now tha player moves where its looking with the camera
            //with this the player can climb in the ladder trigger when the camera is pointing up or down
            Vector3 inputDir = (movementHorizontal * transform.right + movementVertical * headPivotPoint.forward).normalized;
            Vector3 upDownVelocity = inputDir * onLadderSpeed;

            /*
            if (Input.GetButtonDown("Jump"))
            {
                upDownVelocity += -headPivotPoint.forward.normalized * onLadderJumpForce;
                PlayerMovementEnabled(true);
            }*/

            controller.Move(upDownVelocity * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ladderTag))
        {
            PlayerMovementEnabled(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(ladderTag))
        {
            PlayerMovementEnabled(true);
        }
    }

    void PlayerMovementEnabled(bool enabled)
    {
        playerMovement.enabled = enabled;
        playerJetpack.isOn = enabled;
        onLadder = !enabled;
    }
}
