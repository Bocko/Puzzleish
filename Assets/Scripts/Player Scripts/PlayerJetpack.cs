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
    public float jetpackForce = .65f;
    float timeWaited = 0;

    [Header("Exhaust")]
    public Transform leftExhaustPos;
    public Transform rightExhaustPos;
    public ParticleSystem ExhasutParticle;

    [Header("Model")]
    public GameObject jetpackHolder;

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
            jetpackHolder.SetActive(isOn);
            if (playerMovement != null)// when the state changes set the jetpack velocity to 0 and disables the particles
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
        jetpackHolder.SetActive(isOn);
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
        if (!isOn) return;

        bool jetpackDown = Input.GetAxis("Jetpack") == 1;
        playerMovement.SetJetpackVelocity(Vector3.zero);

        if (playerMovement.onGround)
        {
            //if the player is on the ground and the particles are playing stop them
            if (leftEM.enabled && rightEM.enabled)
            {
                leftEM.enabled = false;
                rightEM.enabled = false;
            }
            if (fuel < maxFuel) //if the player is on the ground and current fuel is less than the max wait until the refilldelay is up and than start refilling
            {
                timeWaited += Time.deltaTime;
                if (timeWaited > refillDelay)
                {
                    fuel += fuelRefillRate * Time.deltaTime;
                }
            }
        }
        else //if the player is NOT on the ground and the button for the jetpack is pressed and theres fuel start adding upwards force and start reducing the fuel
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
                playerMovement.SetJetpackVelocity(jetpackForce * Vector3.up);
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
