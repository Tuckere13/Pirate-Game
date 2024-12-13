using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AISteeringManager : MonoBehaviour
{
    public Transform steeringWheel;
    public GameObject Wheel;
    public GameObject player;

    private Vector3 playerPosition;
    private Vector3 playerDirection;
    private float playerDistance;


    private Quaternion targetRotation;

    [Header("Ship Rotation")]
    public float currentWheelRotation = 0.0f;
    [Tooltip("Higher Number = Higher Turn Speed")]
    [Range(0f, .2f)]
    public float shipRotationSpeed = 0.075f;

    private List<Vector3> wayPoints = new List<Vector3>(); // List of all points on path

    [SerializeField] private int maxIterations = 30; // Max iterations for waypoint generation
    private int currentIteration = 0;

    [Tooltip("Lower Number = Higher Accuracy")]
    public int rayCastDistance = 1;
    int pointsOnLine;
    [Tooltip("Radius of circle that police AI will check for collisions")]
    public float checkRadius = 0.5f;


    private int currentWaypointIndex = 0;


    // Colliders to avoid
    private Collider2D playerCollider;
    private Collider2D selfCollider;


    private void Awake()
    {


    }


    private void Update()
    {

        playerPosition = player.transform.position;
        playerDistance = Vector3.Distance(playerPosition, transform.position);

        Pathfind();

        if (wayPoints.Count > 0)
        {
            DecideTurnDirectionBasedOnPath();
        }
    }

    // Determine turn direction based on the next waypoint
    private void DecideTurnDirectionBasedOnPath()
    {
        Vector3 currentPosition = transform.position;

        // Get the next waypoint
        Vector3 nextWaypoint = wayPoints[0]; // Get the first waypoint in the list

        // Calculate the direction to the next waypoint
        Vector3 directionToWaypoint = (nextWaypoint - currentPosition).normalized;

        // Calculate the angle between the current forward direction and the direction to the waypoint
        float angleToWaypoint = Vector3.SignedAngle(transform.forward, directionToWaypoint, Vector3.up);

        // Turn the wheel based on the angle
        if (angleToWaypoint > 5f) // Threshold for turning right
        {
            TurnWheelRight();
        }
        else if (angleToWaypoint < -5f) // Threshold for turning left
        {
            TurnWheelLeft();
        }
        else
        {
            // Reset wheel to straight if the angle is small
            ResetWheel();
        }
    }

    private void ResetWheel()
    {
        if (currentWheelRotation > 0)
        {
            TurnWheelLeft();
        }
        else if (currentWheelRotation < 0)
        {
            TurnWheelRight();
        }
    }



    private void Pathfind()
    {
        wayPoints.Clear(); // Clear the list of waypoints

        int wayPointsNeeded = (int)(playerDistance / rayCastDistance); // Number of waypoints needed
        Vector3 rayCastEndpoint = transform.position + playerDirection * rayCastDistance; // Start position
        wayPoints.Add(rayCastEndpoint); // Add start position to the list

        float angleStep = 10.0f; // The angle to add after a failed collision check

        currentIteration = 0;

        for (int i = 0; i < wayPointsNeeded; i++)
        {

            bool foundClearPath = false;
            float angleOffsetLeft = 0.0f;
            float angleOffsetRight = 0.0f;

            while (!foundClearPath && currentIteration < maxIterations)
            {
                Vector3 previousPoint = rayCastEndpoint; // Store current endpoint if need to reverse

                // Calculate possible endpoints by rotating left and right
                Quaternion leftRotation = Quaternion.AngleAxis(angleOffsetLeft, Vector3.up);
                Quaternion rightRotation = Quaternion.AngleAxis(angleOffsetRight, Vector3.up);

                Vector3 possibleLeftTurn = previousPoint + (leftRotation * playerDirection) * rayCastDistance;
                Vector3 possibleRightTurn = previousPoint + (rightRotation * playerDirection) * rayCastDistance;

                // Check for collisions for both possible directions
                Collider[] hitLeft = Physics.OverlapSphere(possibleLeftTurn, checkRadius);
                Collider[] hitRight = Physics.OverlapSphere(possibleRightTurn, checkRadius);

                bool hasCollisionLeft = HasRelevantCollision(hitLeft);
                bool hasCollisionRight = HasRelevantCollision(hitRight);

                if (hasCollisionLeft)
                {
                    // If there's a collision on the left, increment the left angle offset
                    angleOffsetLeft += angleStep;
                }
                else if (hasCollisionRight)
                {
                    // If there's a collision on the right, decrement the right angle offset
                    angleOffsetRight -= angleStep;
                }

                // Decide which side to turn based off which side wouldn't collide
                if (!hasCollisionLeft && !hasCollisionRight)
                {
                    // Choose closest direction if both are good
                    if (Mathf.Abs(angleOffsetLeft) <= Mathf.Abs(angleOffsetRight))
                    {
                        rayCastEndpoint = possibleLeftTurn;
                    }
                    else
                    {
                        rayCastEndpoint = possibleRightTurn;
                    }
                    foundClearPath = true;
                }
                else if (!hasCollisionLeft)
                {
                    // Only the left direction is clear
                    rayCastEndpoint = possibleLeftTurn;
                    foundClearPath = true;
                }
                else if (!hasCollisionRight)
                {
                    // Only the right direction is clear
                    rayCastEndpoint = possibleRightTurn;
                    foundClearPath = true;
                }
                else
                {
                    // If both directions are blocked, continue adjusting angles
                    angleOffsetLeft += angleStep;
                    angleOffsetRight -= angleStep;
                }

                currentIteration++; // Prevent excessive waypoint generation

                // FOR VISUAL TESTING //
                if (foundClearPath)
                {
                    wayPoints.Add(rayCastEndpoint);
                    // Uncomment if you have a visualization system
                    // GameObject waypoint = Instantiate(waypointSphere, rayCastEndpoint, Quaternion.identity);
                    // waypointObjects.Add(waypoint);
                }
            }
        }

        foreach (var point in wayPoints)
        {
            Debug.Log("Waypoint: " + point);
        }
    }

    // Helper method to check for relevant collisions
    private bool HasRelevantCollision(Collider[] hitColliders)
    {
        foreach (Collider collider in hitColliders)
        {
            if (collider != playerCollider && collider != selfCollider)
            {
                return true; // Found a relevant collision
            }
        }
        return false; // No relevant collisions found
    }

    private void TurnWheelRight()
    {

            if (currentWheelRotation <= 1.0f)
            {
                currentWheelRotation += shipRotationSpeed * Time.deltaTime;

                Wheel.transform.Rotate(Vector3.up, -60.0f * Time.deltaTime);
            }

    }


    private void TurnWheelLeft()
    {
 
        if (currentWheelRotation >= -1.0f)
        {
            currentWheelRotation += -shipRotationSpeed * Time.deltaTime;

            Wheel.transform.Rotate(Vector3.up, 60.0f * Time.deltaTime);
        }

    }


}

