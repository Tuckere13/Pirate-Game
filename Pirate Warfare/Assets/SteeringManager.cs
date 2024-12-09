using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SteeringManager : MonoBehaviour
{
    public Transform steeringWheel;
    public GameObject player;
    public GameObject Wheel;

    public Transform playerStandingPosition;  // Where player stands when using wheel
    public Transform playerLookPoint; // where player looks when using wheel  ***** NOT IN USE *****

    private bool playerUsingWheel = false;

    private bool canUseWheel = false;

    [Header("Steering Wheel Use")] 
    public float facingThreshold = 0.8f;
    public float interactionDistance = 2.0f;
    public float interactionHeightTolerance = 3.0f;

    //private bool isRotating = false;
    private Quaternion targetRotation;

    private PlayerMovement playerMovement;

    
    [Header("Ship Rotation")]
    public float currentWheelRotation = 0.0f;
    [Tooltip("Higher Number = Higher Turn Speed")]
    [Range(0f, .2f)]
    public float shipRotationSpeed = 0.075f;


    [Header("UI Popup")]
    public GameObject sailUI;
    public GameObject RightCannonUI;
    public GameObject LeftCannonUI;
    private bool showSailUI = false;
    private bool showRightCannonUI = false;
    private bool showLeftCannonUI = false;



    private void Awake()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        playerLookPoint = GetComponentInChildren<Transform>();


        showSailUI = false;
        showRightCannonUI = false;
        showLeftCannonUI = false;

    }


    private void Update()
    {
        CheckCanUseWheel();

        //Debug.Log($"showSailUI: {showSailUI}, showRightCannonUI: {showRightCannonUI}, showLeftCannonUI: {showLeftCannonUI}");

        if (Input.GetKeyDown(KeyCode.F) && canUseWheel)
        {
            playerUsingWheel = !playerUsingWheel;   // switch from true to false or vice versa
            /*showSailUI = !showSailUI;
            showRightCannonUI = !showRightCannonUI;
            showLeftCannonUI = !showLeftCannonUI;*/

            playerMovement.usingSteeringWheel = playerUsingWheel;

        }


        if (playerUsingWheel)
        {

            player.transform.position = playerStandingPosition.position;

            float screenWidth = Screen.width;

            Vector3 mousePosition = Input.mousePosition;



            /*
            if (mousePosition.x > screenWidth * (4f / 5f))
            {
                playerMovement.LookAtWheel(transform.position, 15); // Right turn

                showSailUI = false;
                showRightCannonUI = true;
                showLeftCannonUI = false;
            }
            else if (mousePosition.x < screenWidth / 5f)
            {
                playerMovement.LookAtWheel(transform.position, -15); // Left turn

                showSailUI = false;
                showRightCannonUI = false;
                showLeftCannonUI = true;
            }
            else
            {
                playerMovement.LookAtWheel(transform.position, -.5f); // No turn
                showSailUI = true;
                showRightCannonUI = true;
                showLeftCannonUI = true;
            }
            */
            if (mousePosition.x > screenWidth * (4f / 5f))
            {
                // Look slightly downward and to the right
                playerMovement.LookAtWheel(transform.position, 45, 35); // 25 degrees to the right
                showSailUI = false;
                showRightCannonUI = true;
                showLeftCannonUI = false;
            }
            else if (mousePosition.x < screenWidth / 5f)
            {
                // Look slightly downward and to the left
                playerMovement.LookAtWheel(transform.position, -45, 35); // 25 degrees to the left
                showSailUI = false;
                showRightCannonUI = false;
                showLeftCannonUI = true;
            }
            else
            {
                // Look slightly downward and forward
                playerMovement.LookAtWheel(transform.position, 0, 35); // Slightly downward
                showSailUI = true;
                showRightCannonUI = true;
                showLeftCannonUI = true;
            }



            TurnWheel();


            // Make the cursor visible and unlock it
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (showSailUI)
            {
                sailUI.SetActive(true); // Activate the pop-up
            } else
            {
                sailUI.SetActive(false);
            }
            if (showRightCannonUI)
            {
                RightCannonUI.SetActive(true); // Activate the pop-up
            }
            else
            {
                RightCannonUI.SetActive(false);
            }
            if (showLeftCannonUI)
            {
                LeftCannonUI.SetActive(true); // Activate the pop-up
            }
            else
            {
                LeftCannonUI.SetActive(false);
            }
        }
        else
        {
            // Hide the cursor and lock it to the center of the screen
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;


            if (sailUI != null)
            {
                sailUI.SetActive(false); // Deactivate the pop-up
            }
            if (RightCannonUI != null)
            {
                RightCannonUI.SetActive(false); // Deactivate the pop-up
            }
            if (LeftCannonUI != null)
            {
                LeftCannonUI.SetActive(false); // Deactivate the pop-up
            }
        }

        
    }

    

    private void CheckCanUseWheel()
    {
        Vector3 toSteeringWheel = (steeringWheel.position - player.transform.position).normalized;
        

        if (playerUsingWheel)
        {
            canUseWheel = true;
            return;
            
        }

        
        // check if player is facing steering wheel
        float dot = Vector3.Dot(player.transform.forward, toSteeringWheel);
        if (dot < facingThreshold)
        {
            canUseWheel = false;
            UnityEngine.Debug.Log("Here DOT");
            return;
            
        }
        

        // check if player is within a certain distance from wheel
        float distance = Vector3.Distance(player.transform.position, steeringWheel.position);
        if (distance > interactionDistance)
        {
            canUseWheel = false;
            UnityEngine.Debug.Log("Here Distance");
            return;
        }

        // check if player is on same deck as the wheel within an amount
        if (Mathf.Abs(player.transform.position.y - steeringWheel.position.y) > interactionHeightTolerance)
        {
            canUseWheel = false;
            UnityEngine.Debug.Log("Here Height");
            return;
        }
        canUseWheel = true;

        return;
    }


    private void TurnWheel()
    {

        if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.D))
        {
            currentWheelRotation += 0.0f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (currentWheelRotation >= -1.0f)
            {
                currentWheelRotation += -shipRotationSpeed * Time.deltaTime;

                Wheel.transform.Rotate(Vector3.up, 60.0f * Time.deltaTime);
            }

            

        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (currentWheelRotation <= 1.0f)
            {
                currentWheelRotation += shipRotationSpeed * Time.deltaTime;

                Wheel.transform.Rotate(Vector3.up, -60.0f * Time.deltaTime);
            }
        }

    }

    
}
