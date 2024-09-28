using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovement : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 startPosition;
    private float timer;
    public float timeLimit = 600f;

    void Start()
    {
        
        startPosition = transform.position;

        timer = 0f;
    }
    void Update()
    {
        Vector3 newPosition = transform.position + new Vector3(2,0,-1) * moveSpeed * Time.deltaTime;

        transform.position = newPosition;

        timer += Time.deltaTime;

        if (timer >= timeLimit)
        {
            transform.position = startPosition;

            timer = 0f;
        }
    }
}
