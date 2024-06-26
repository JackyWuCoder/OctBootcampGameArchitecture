using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float moveSpeed = 10.0f;//Y
    [SerializeField] private float turnSpeed = 10.0f;//Y
    [SerializeField] private Transform cameraTransform;//-
    [SerializeField] private bool invertMouse;//Y
    [SerializeField] private float sprintMultiplier = 2.0f; //Y

    [Header("Player Jump")]
    [SerializeField] private Transform groundCheck;//Y
    [SerializeField] private LayerMask groundMask;//Y
    [SerializeField] private float groundCheckDistance;//Y
    [SerializeField] private float gravity = -9.81f;//Y
    [SerializeField] private float jumpVelocity;//Y

    [Header("Player Shoot")]
    [SerializeField] private Rigidbody bulletPrefab;//Y
    [SerializeField] private Rigidbody rocketPrefab;//Y
    [SerializeField] private float shootForce;//Y
    [SerializeField] private Transform shootPoint;//Y

    [Header("Select Interaction")]
    [SerializeField] private Camera cam;//Y
    [SerializeField] private LayerMask interactionLayerMask;//Y
    [SerializeField] private float interactionDistance;//Y

    [Header("Pickup Interaction")]
    [SerializeField] private LayerMask pickupLayerMask;//Y
    [SerializeField] private float pickupDistance;//Y
    [SerializeField] private Transform attachTransform;//Y

    private CharacterController characterController;//Y
    private float horizontalInput, verticalInput;//Y
    private float mouseX, mouseY;//Y
    private float moveMultiplier = 1.0f;//Y
    private float camXRotation;//Y
    private bool isGrounded;//Y
    private Vector3 playerVelocity;//Y

    // Interaction Raycasts
    private RaycastHit hit;//Y
    private ISelectable selection;//Y
    private IPickable pickable;//Y

    private bool isPicked = false; //Y

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();//Y
    }

    // Start is called before the first frame update
    void Start()
    {
        //Hide Mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }//Y

    // Update is called once per frame
    void Update()
    {
        GetInput();
        RotatePlayer();

        GroundCheck();
        MovePlayer();
        JumpCheck();

        Shoot();
        Interact();
        PickAndDrop();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");//Y
        verticalInput = Input.GetAxis("Vertical");//Y
        mouseX = Input.GetAxis("Mouse X");//Y
        mouseY = Input.GetAxis("Mouse Y");//Y
        moveMultiplier = Input.GetButton("Sprint") ? sprintMultiplier : 1.0f;//Y
    }//Y

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
    }//Y

    private void RotatePlayer()
    {
        //Player turn movement
        transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime * mouseX);//Y

        //Camera up/down movement
        camXRotation += Time.deltaTime * mouseY * turnSpeed * (invertMouse ? 1 : -1);
        camXRotation = Mathf.Clamp(camXRotation, -50.0f, 50.0f); // Restricts up/down rotation to 50 degrees
        
        cameraTransform.localRotation = Quaternion.Euler(camXRotation, 0, 0);
    }//Y

    private void GroundCheck()
    {
        // A sphere, centered at position of ground, with radius of checkDistance, where groundMask is the collision object,
        // of ground we are checking.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);
    }//Y

    private void JumpCheck()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerVelocity.y = jumpVelocity;
        }
    }//Y

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
        if (Input.GetButtonDown("Fire2"))
        {
            Rigidbody bullet = Instantiate(rocketPrefab, shootPoint.position, shootPoint.rotation);
            // ForceMode.Impulse allows the bullet to travel a lot faster
            bullet.AddForce(shootPoint.forward * shootForce, ForceMode.Impulse);
            Destroy(bullet.gameObject, 5.0f);
        }
    }//Y

    private void Interact()
    {
        // Cast a ray from middle of camera.
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2));
        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayerMask))
        {
            Debug.Log("We hit " + hit.collider.name);
            Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red);
            selection = hit.transform.GetComponent<ISelectable>();
            if (selection != null)
            {
                selection.OnHoverEnter();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    selection.OnSelect();
                }
            }
        }
        if (hit.transform == null && selection != null)
        {
            selection.OnHoverExit();
            selection = null;
        }
    }

    private void PickAndDrop()
    {
        // Cast a ray
        if (Physics.Raycast(GetCamRay(), out hit, pickupDistance, pickupLayerMask))
        {
            if (Input.GetKeyDown(KeyCode.E) && !isPicked)
            {
                pickable = hit.transform.GetComponent<IPickable>();
                if (pickable == null) return;
                pickable.OnPicked(attachTransform);
                isPicked = true;
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.E) && isPicked && pickable != null)
        {
            pickable.OnDropped();
            isPicked = false;
        }
    }//Y

    private Ray GetCamRay()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        return ray;
    }//Y
}
