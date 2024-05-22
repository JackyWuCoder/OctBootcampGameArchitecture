using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class PlayerInput : MonoBehaviour
{
    public float horizontal { get; private set; }
    public float vertical { get; private set; }
    public float mouseX { get; private set; }
    public float mouseY { get; private set; }
    public bool sprintHeld { get; private set; }
    public bool jumpPressed { get; private set; }
    public bool primaryButtonPressed { get; private set; } // Left Click
    public bool secondaryButtonPressed { get; private set; } // Right Click
    public bool activatePressed { get; private set; }

    public bool clear;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ClearInputs();
        ProcessInputs();
    }

    private void FixedUpdate()
    {
        clear = true;
    }

    void ClearInputs()
    {
        if (!clear) return;
        horizontal = 0;
        vertical = 0;
        mouseX = 0;
        mouseY = 0;

        sprintHeld = false;
        jumpPressed = false;

        activatePressed = false;
        primaryButtonPressed = false;
        secondaryButtonPressed = false;
    }

    void ProcessInputs()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        sprintHeld = sprintHeld || Input.GetButton("Sprint");
        jumpPressed = jumpPressed | Input.GetButtonDown("Jump");

        activatePressed = activatePressed || Input.GetKeyDown(KeyCode.E);
        primaryButtonPressed = primaryButtonPressed || Input.GetButton("Fire1");
        secondaryButtonPressed = secondaryButtonPressed || Input.GetButton("Fire2");
    }
}
