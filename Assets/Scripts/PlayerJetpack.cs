using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJetpack : MonoBehaviour
{
    public float maxFuel = 100;
    public float fuel;
    public float fuelRate;
    public float fuelRefillRate;
    public float refillDelay;
    public float jetpackForce = 100;

    PlayerMovement playerMovement;
    Rigidbody playerRB;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerRB = GetComponent<Rigidbody>();
        fuel = maxFuel;
    }

    // Update is called once per frame
    void Update()
    {
        bool jumpDown = Input.GetAxis("Jump") == 1;

        if(!playerMovement.onGround && jumpDown)
        {
            playerRB.AddForce(jetpackForce * Time.deltaTime * Vector3.up);
            print(jetpackForce * Time.deltaTime * Vector3.up);
        }
    }
}
