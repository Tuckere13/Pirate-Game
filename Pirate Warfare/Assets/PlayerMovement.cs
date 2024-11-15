using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool CanMove { get; private set; } = true;

    [Header("Look Parameters")]
    [SerializeField, Range(1f, 10f)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1f, 10f)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1f, 100f)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1f, 100f)] private float lowerLookLimit = 80.0f;

    [Header("Movement Parameters")]
    [SerializeField] private float playerBaseSpeed = 80.0f;
    [SerializeField] private float gravity = 30.0f;

    private float playerSpeed;
    private float playerSprintSpeed;
    

    private Camera playerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector3 currentIndex;

    private float rotationX = 0;
    private Vector2 currentInput;

    public bool usingSteeringWheel = false;

    [Header("Ship Interaction")]
    public Rigidbody shipRigidbody; // Reference to the ship's Rigidbody
    private bool isOnShip = false;

    private void Start()
    {
        playerSprintSpeed = (float)(playerBaseSpeed + (playerBaseSpeed * .5));

    }

    void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Calculate ship movement since last frame

        if (!usingSteeringWheel)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                playerSpeed = playerSprintSpeed;
            }
            else
            {
                playerSpeed = playerBaseSpeed;
            }

            if (CanMove)
            {
                HandleMovementInput();
            }
        }

        if (CanMove && !usingSteeringWheel)
        {
            ApplyFinalMovement();
        }

        HandleMouseLook();
    }

    private void HandleMovementInput()
    {
        currentInput = new Vector2(playerSpeed * Input.GetAxis("Vertical"), playerSpeed * Input.GetAxis("Horizontal"));

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    private void ApplyFinalMovement()
    {
        if (!characterController.isGrounded) 
            moveDirection.y -= gravity * Time.deltaTime;
        

        characterController.Move(moveDirection * Time.deltaTime);
    }
}