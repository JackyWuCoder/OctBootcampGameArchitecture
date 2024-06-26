using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovementBehaviour : MonoBehaviour
{
    private PlayerInput playerInput;

    [Header("Player Camera Movement")]
    [SerializeField] private float turnSpeed = 10.0f;
    [SerializeField] private bool invertMouse;

    private float camXRotation;

    // Start is called before the first frame update
    void Start()
    {
        //Hide Mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerInput = PlayerInput.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        //Camera up/down movement
        camXRotation += Time.deltaTime * playerInput.mouseY * turnSpeed * (invertMouse ? 1 : -1);
        camXRotation = Mathf.Clamp(camXRotation, -50.0f, 50.0f); // Restricts up/down rotation to 50 degrees

        transform.localRotation = Quaternion.Euler(camXRotation, 0, 0);
    }
}
