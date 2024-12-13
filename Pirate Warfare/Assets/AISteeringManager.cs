using System;
using System.Collections.Generic;
using UnityEngine;

public class AISteeringManager : MonoBehaviour
{
    public Transform aiShipFrontPoint;
    public GameObject ship;
    public GameObject Wheel;

    public List<Transform> targetPoints; // List of potential target points
    private Transform currentTarget;

    public float turningThreshold = 1.0f; // Angle threshold for stopping the turn
    public float movementSpeed = 5.0f; // Movement speed of the ship
    public float stoppingDistance = 1.5f; // Distance to stop near the target point

    public Vector3 randomPointToGo;

    [Header("Ship Rotation")]
    public float AIcurrentWheelRotation = 0.0f;
    [Tooltip("Higher Number = Higher Turn Speed")]
    [Range(0f, .2f)]
    public float shipRotationSpeed = 0.075f;

    private void Start()
    {

        UnityEngine.Random.InitState(Environment.TickCount);


        randomPointToGo = SelectNewTarget();
    }
    private void Update()
    {
        // Calculate the direction to the random point
        Vector3 dirToRandomPoint = randomPointToGo - transform.position;

        // Ignore Z-axis (project onto the XY plane)
        Vector2 dirToRandomPointXY = new Vector2(dirToRandomPoint.x, dirToRandomPoint.y);
        Vector2 forwardXY = new Vector2(transform.forward.x, transform.forward.y);

        // Check if the distance to the random point is greater than 50
        if (Vector3.Distance(transform.position, randomPointToGo) >= 1000)
        {
            // Use the 2D cross product to determine left or right
            float cross = forwardXY.x * dirToRandomPointXY.y - forwardXY.y * dirToRandomPointXY.x;

            if (cross > 0)
            {
                // Point is to the left
                TurnWheelLeft();
                //Debug.Log("Point is to the LEFT of the object.");
            }
            else if (cross < 0)
            {
                TurnWheelRight();
                // Point is to the right
                //Debug.Log("Point is to the RIGHT of the object.");
            }
            else
            {
                ResetWheel();
                // Point is directly in front or behind
                //Debug.Log("Point is directly in line with the forward direction.");
            }
        }
        else
        {
            UnityEngine.Debug.Log("New point selected");
            randomPointToGo = SelectNewTarget();
        }
    }


    private Vector3 SelectNewTarget()
    {
        float randomX = GetRandomNumberExcludingRange(-5000f, 5000f, -3000f, 3000f);
        float randomY = GetRandomNumberExcludingRange(-5000f, 5000f, -3000f, 3000f);

        Vector3 searchPoint = new Vector3(transform.position.x + randomX, transform.position.y + randomY, aiShipFrontPoint.transform.position.z);

        return searchPoint;
    }


    float GetRandomNumberExcludingRange(float min, float max, float excludeMin, float excludeMax)
    {
        float randomValue;
        do
        {
            randomValue = UnityEngine.Random.Range(min, max);
        }
        while (randomValue > excludeMin && randomValue < excludeMax);

        return randomValue;
    }


    private void ResetWheel()
    {
        AIcurrentWheelRotation = 0;
    }



    /*
    private void Pathfind()
    {
        wayPoints.Clear(); // Clear the list of waypoints

        // Use ship's position as the starting point
        Vector3 rayCastEndpoint = ship.transform.position + playerDirection * rayCastDistance; // Start position
        wayPoints.Add(rayCastEndpoint); // Add start position to the list

        float angleStep = 10.0f; // The angle to add after a failed collision check
        currentIteration = 0;

        for (int i = 0; i < (int)(playerDistance / rayCastDistance); i++)
        {
            bool foundClearPath = false;
            float angleOffsetLeft = 0.0f;
            float angleOffsetRight = 0.0f;

            while (!foundClearPath && currentIteration < maxIterations)
            {
                Vector3 previousPoint = rayCastEndpoint; // Use the current rayCastEndpoint for calculations

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
                    // Increment left angle offset if there's a collision
                    angleOffsetLeft += angleStep;
                }
                else if (hasCollisionRight)
                {
                    // Decrement right angle offset if there's a collision
                    angleOffsetRight -= angleStep;
                }

                // Determine which direction is clear
                if (!hasCollisionLeft && !hasCollisionRight)
                {
                    rayCastEndpoint = Mathf.Abs(angleOffsetLeft) <= Mathf.Abs(angleOffsetRight)
                        ? possibleLeftTurn
                        : possibleRightTurn;
                    foundClearPath = true;
                }
                else if (!hasCollisionLeft)
                {
                    rayCastEndpoint = possibleLeftTurn;
                    foundClearPath = true;
                }
                else if (!hasCollisionRight)
                {
                    rayCastEndpoint = possibleRightTurn;
                    foundClearPath = true;
                }
                else
                {
                    // Adjust angles further if both directions are blocked
                    angleOffsetLeft += angleStep;
                    angleOffsetRight -= angleStep;
                }

                currentIteration++; // Increment iteration count to avoid infinite loops

                if (foundClearPath)
                {
                    wayPoints.Add(rayCastEndpoint); // Add the valid endpoint to the waypoints
                }
            }
        }

        // Visualize waypoints for debugging
        foreach (var point in wayPoints)
        {
            Debug.DrawLine(ship.transform.position, point, Color.red, 0.5f);
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
    */

    private void TurnWheelRight()
    {
        if (AIcurrentWheelRotation < .50f) // Limit max rotation
        {
            AIcurrentWheelRotation += shipRotationSpeed * Time.deltaTime;
            Wheel.transform.Rotate(Vector3.up, -60.0f * Time.deltaTime);
        }
    }

    private void TurnWheelLeft()
    {
        if (AIcurrentWheelRotation > -.50f) // Limit min rotation
        {
            AIcurrentWheelRotation -= shipRotationSpeed * Time.deltaTime;
            Wheel.transform.Rotate(Vector3.up, 60.0f * Time.deltaTime);
        }
    }



}

