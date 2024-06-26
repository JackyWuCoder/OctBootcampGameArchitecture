using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBehaviour : MonoBehaviour
{
    private PlayerInput playerInput;

    [Header("Player Movement")]
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float sprintMultiplier = 2.0f;

    [Header("Ground Check")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckDistance;

    private CharacterController characterController;
    private Vector3 playerVelocity;
    public bool isGrounded { get; private set; }
    private float moveMultiplier = 1.0f;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = PlayerInput.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        MovePlayer();
    }

    private void GroundCheck()
    {
        // A sphere, centered at position of ground, with radius of checkDistance, where groundMask is the collision object,
        // of ground we are checking.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);
    }

    private void MovePlayer()
    {
        moveMultiplier = playerInput.sprintHeld ? sprintMultiplier : 1.0f;
        
        characterController.Move((transform.forward * playerInput.vertical + transform.right * playerInput.horizontal) * moveSpeed * moveMultiplier * Time.deltaTime);
       
        //Ground Check
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    public void SetYVelocity(float value)
    {
        playerVelocity.y = value;
    }

    public float GetForwardSpeed()
    {
        return playerInput.vertical * moveSpeed * moveMultiplier;
    }
}
