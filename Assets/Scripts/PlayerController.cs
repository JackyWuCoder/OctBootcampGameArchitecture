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

    [Header("Player Jump")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpVelocity;

    [Header("Player Shoot")]
    [SerializeField] private Rigidbody bulletPrefab;
    [SerializeField] private float shootForce;
    [SerializeField] private Transform shootPoint;

    private CharacterController characterController;
    private float horizontalInput, verticalInput;
    private float mouseX, mouseY;
    private float moveMultiplier = 1.0f;
    private float camXRotation;
    private bool isGrounded;
    private Vector3 playerVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Hide Mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        RotatePlayer();

        GroundCheck();
        MovePlayer();
        JumpCheck();

        Shoot();
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
        //Ground Check
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    private void RotatePlayer()
    {
        //Player turn movement
        transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime * mouseX);

        //Camera up/down movement
        camXRotation += Time.deltaTime * mouseY * turnSpeed * (invertMouse ? 1 : -1);
        camXRotation = Mathf.Clamp(camXRotation, -50.0f, 50.0f); // Restricts up/down rotation to 50 degrees
        cameraTransform.localRotation = Quaternion.Euler(camXRotation, 0, 0);
    }

    private void GroundCheck()
    {
        // A sphere, centered at position of ground, with radius of checkDistance, where groundMask is the collision object,
        // of ground we are checking.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);
    }

    private void JumpCheck()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerVelocity.y = jumpVelocity;
        }
    }

    // Used to check the visibility for collision using GroundCheck()
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(groundCheck.position, groundCheckDistance);
    }
  
    private void Shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Rigidbody bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            // ForceMode.Impulse allows the bullet to travel a lot faster
            bullet.AddForce(shootPoint.forward * shootForce, ForceMode.Impulse);
            Destroy(bullet.gameObject, 5.0f);
        }
    }
}
