using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementRB : MonoBehaviour
{
    public bool CanMove { get; private set; } = true;

    [Header("Look Parameters")]
    [SerializeField, Range(1f, 10f)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1f, 10f)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1f, 100f)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1f, 100f)] private float lowerLookLimit = 80.0f;

    [Header("Movement Parameters")]
    [SerializeField] private float playerBaseSpeed = 5.0f;
    [SerializeField] private float jumpForce = 5.0f;

    private float playerSpeed;
    private float playerSprintSpeed;

    private Camera playerCamera;
    private Rigidbody playerRigidbody;

    private Vector3 movementInput;

    private float rotationX = 0;
    private bool isOnShip = false;

    [Header("Ship Interaction")]
    public Rigidbody shipRigidbody; // Reference to the ship's Rigidbody
    private FixedJoint shipJoint; // Joint to hold player on ship

    private void Start()
    {
        playerSprintSpeed = playerBaseSpeed * 1.5f;
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!CanMove) return;

        // Check for sprinting
        if (Input.GetKey(KeyCode.LeftShift))
            playerSpeed = playerSprintSpeed;
        else
            playerSpeed = playerBaseSpeed;

        HandleMovementInput();
        HandleMouseLook();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void HandleMovementInput()
    {
        // Get input for movement and apply it
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        movementInput = new Vector3(horizontal, 0, vertical).normalized * playerSpeed;
    }

    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    private void ApplyMovement()
    {
        // Apply movement relative to the player's forward direction
        Vector3 move = transform.TransformDirection(movementInput) * Time.fixedDeltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + move);

        // Jumping
        if (Input.GetButtonDown("Jump") && isOnShip)
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ship")) // Ensure the ship has the tag "Ship"
        {
            AttachToShip();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ship"))
        {
            DetachFromShip();
        }
    }

    private void AttachToShip()
    {
        isOnShip = true;

        // Add a fixed joint to "attach" the player to the ship
        shipJoint = gameObject.AddComponent<FixedJoint>();
        shipJoint.connectedBody = shipRigidbody;
    }

    private void DetachFromShip()
    {
        isOnShip = false;

        // Remove the joint to detach from the ship
        if (shipJoint != null)
        {
            Destroy(shipJoint);
        }
    }
}
