using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJetpack : MonoBehaviour
{
    public float maxFuel = 100;
    public float fuel;
    public float fuelBurnRate = 20;
    public float fuelRefillRate = 10;
    public float refillDelay = 1;
    public float jetpackForce = 40;
    float timeWaited = 0;

    bool isOn;
    public bool IsOn
    {
        get { return isOn; }
        set
        {
            isOn = value;
            if (playerMovement != null)
            {
                playerMovement.SetJetpackVelocity(Vector3.zero);
            }
        }
    }

    PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        fuel = maxFuel;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            bool jetpackDown = Input.GetAxis("Jetpack") == 1;
            playerMovement.SetJetpackVelocity(Vector3.zero);

            if (playerMovement.onGround)
            {
                if (fuel < maxFuel)
                {
                    timeWaited += Time.deltaTime;
                    if (timeWaited > refillDelay)
                    {
                        fuel += fuelRefillRate * Time.deltaTime;
                    }
                }
            }
            else
            {
                timeWaited = 0;
                if (jetpackDown && fuel > 0)
                {
                    playerMovement.SetJetpackVelocity(jetpackForce * Time.deltaTime * Vector3.up);
                    fuel -= fuelBurnRate * Time.deltaTime;
                }
            }
        }
    }
}
