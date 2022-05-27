using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float mouseSens = 1;

    public float MouseX;
    public float MouseY;
    public float MouseXPlayerSens
    {
        get
        {
            return MouseX * mouseSens;
        }
    }
    public float MouseYPlayerSens
    {
        get
        {
            return MouseY * mouseSens;
        }
    }

    public float MouseWheel;
    public bool Fire1Down;
    public bool Fire2Down;
    public bool Fire3Down;
    public bool Fire1Up;
    public bool Fire3Up;

    public float Horizontal;
    public float Vertical;
    public bool Jump;
    public bool Crouch;
    public bool Walk;
    public bool Jetpack;
    public bool TimeTravel;

    void Update()
    {
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");
        MouseWheel = Input.GetAxisRaw("Mouse ScrollWheel");
        Fire1Down = Input.GetButtonDown("Fire1");
        Fire2Down = Input.GetAxisRaw("Fire2") == 1;
        Fire3Down = Input.GetAxisRaw("Fire3") == 1;
        Fire1Up = Input.GetButtonUp("Fire1");
        Fire3Up = Input.GetButtonUp("Fire3");

        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
        Jump = Input.GetButtonDown("Jump");
        Crouch = Input.GetAxisRaw("Crouch") == 1;
        Walk = Input.GetAxisRaw("Walk") == 1;
        Jetpack = Input.GetAxisRaw("Jetpack") == 1;
        TimeTravel = Input.GetButtonDown("TimeTravel");
    }
}