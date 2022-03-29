using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJetpack : MonoBehaviour
{
    [Header("Jetpacky Stuff")]
    public bool isOnAtStart = false;
    public float maxFuel = 100;
    public float fuel;
    public float fuelBurnRate = 20;
    public float fuelRefillRate = 10;
    public float refillDelay = 1;
    public float jetpackForce = 40;
    float timeWaited = 0;

    [Header("Exhaust")]
    public Transform leftExhaustPos;
    public Transform rightExhaustPos;
    public ParticleSystem ExhasutParticle;

    ParticleSystem leftParticleSpawned;
    ParticleSystem rightParticleSpawned;
    //this is a shit workaround of getting the particle system to play before the previously emmited particles disapear
    //basically the particle system plays non stop but theres no emmision because the module of it is disabled...
    ParticleSystem.EmissionModule leftEM;
    ParticleSystem.EmissionModule rightEM;

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
                leftEM.enabled = false;
                rightEM.enabled = false;
            }
        }
    }

    PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        fuel = maxFuel;
        isOn = isOnAtStart;
        leftParticleSpawned = Instantiate(ExhasutParticle, leftExhaustPos);
        rightParticleSpawned = Instantiate(ExhasutParticle, rightExhaustPos);
        leftEM = leftParticleSpawned.emission;
        rightEM = rightParticleSpawned.emission;
        leftEM.enabled = false;
        rightEM.enabled = false;
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
                if(leftEM.enabled && rightEM.enabled)
                {
                    leftEM.enabled = false;
                    rightEM.enabled = false;
                }
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
                    /*
                    if(!leftParticleSpawned.isPlaying && !rightParticleSpawned.isPlaying)
                    {
                        leftParticleSpawned.Play();
                        rightParticleSpawned.Play();
                    }*/
                    leftEM.enabled = true;
                    rightEM.enabled = true;
                    playerMovement.SetJetpackVelocity(jetpackForce * Time.deltaTime * Vector3.up);
                    fuel -= fuelBurnRate * Time.deltaTime;
                }
                else
                {
                    /*
                    if (leftParticleSpawned.isPlaying && rightParticleSpawned.isPlaying)
                    {
                        leftParticleSpawned.Stop();
                        rightParticleSpawned.Stop();
                    }*/
                    leftEM.enabled = false;
                    rightEM.enabled = false;
                }
            }
        }
    }
}
