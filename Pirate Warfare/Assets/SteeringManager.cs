using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SteeringManager : MonoBehaviour
{
    public Transform steeringWheel;
    public GameObject player;

    public Transform playerStandingPosition;  // Where player stands when using wheel

    private bool playerUsingWheel = false;

    private bool canUseWheel = false;

    [Header("Steering Wheel Use")] 
    public float facingThreshold = 0.8f;
    public float interactionDistance = 2.0f;
    public float interactionHeightTolerance = 3.0f;

    private bool isRotating = false;
    private Quaternion targetRotation;

    private PlayerMovement playerMovement;

    
    [Header("Ship Rotation")]
    public float currentWheelRotation = 0.0f;
    [Tooltip("Higher Number = Higher Turn Speed")]
    [Range(0f, .2f)]
    public float shipRotationSpeed = 0.075f;
    


    private void Awake()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
    }


    private void Update()
    {

        checkCanUseWheel();

        if (Input.GetKeyDown(KeyCode.F) && canUseWheel)
        {
            playerUsingWheel = !playerUsingWheel;   // switch from true to false or vice versa

            //Vector3 directionToWheel = (steeringWheel.position - player.transform.position).normalized;
            //targetRotation = Quaternion.LookRotation(directionToWheel);
            //isRotating = true;

            playerMovement.usingSteeringWheel = playerUsingWheel;
        }


        if (playerUsingWheel)
        {
            player.transform.position = playerStandingPosition.position;


            TurnWheel();


            // Show UI Here ***********

        }

        if (isRotating && playerUsingWheel)
        {
            player.transform.rotation = targetRotation;

            // Stop rotating when the player is close enough to the target rotation
            if (Quaternion.Angle(player.transform.rotation, targetRotation) < 1f)
            {
                player.transform.rotation = targetRotation;

                Vector3 newEulerAngles = player.transform.eulerAngles;
                newEulerAngles.z = 0f; // Set x rotation to upright
                newEulerAngles.x = 0f; // Set y rotation to face forward

                player.transform.eulerAngles = newEulerAngles;

                isRotating = false;
            }
        }
    }

    

    private void checkCanUseWheel()
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
            return;
        }

        // check if player is within a certain distance from wheel
        float distance = Vector3.Distance(player.transform.position, steeringWheel.position);
        if (distance > interactionDistance)
        {
            canUseWheel = false;
            return;
        }

        // check if player is on same deck as the wheel within an amount
        if (Mathf.Abs(player.transform.position.y - steeringWheel.position.y) <= interactionHeightTolerance)// Adjust tolerance as needed
        {
            canUseWheel = false;
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
            }
            
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (currentWheelRotation <= 1.0f)
            {
                currentWheelRotation += shipRotationSpeed * Time.deltaTime;
            }
        }

        UnityEngine.Debug.Log(currentWheelRotation);
    }


}
