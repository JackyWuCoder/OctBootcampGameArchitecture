using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float turnSpeed = 10.0f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private bool invertMouse;
    [SerializeField] private float sprintMultiplier = 2.0f;

    private CharacterController characterController;
    private float horizontalInput, verticalInput;
    private float mouseX, mouseY;
    private float moveMultiplier = 1.0f;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        MovePlayer();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        moveMultiplier = Input.GetButton("Sprint") ? sprintMultiplier : 1.0f;
    }

    private void MovePlayer()
    {
        characterController.Move((transform.forward * verticalInput + transform.right * horizontalInput) * moveSpeed * moveMultiplier * Time.deltaTime);
    }

    private bool isSprint()
    {
        return (Input.GetKeyDown(KeyCode.LeftShift) == true) ? true : false;
    }
}
