using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBoatMovement : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 startPosition;
    private float timer;
    public float timeLimit;

    public float bobbingAmplitude = 0.5f;
    public float bobbingFrequency = 1.0f;

    void Start()
    {
        startPosition = transform.position;

        timer = 0f;
    }
    void Update()
    {
        Vector3 newPosition = transform.position + new Vector3(90, 0, -1) * moveSpeed * Time.deltaTime;

        transform.position = newPosition;

        timer += Time.deltaTime;


        float bobbingOffset = Mathf.Sin(Time.time * bobbingFrequency) * bobbingAmplitude;

        transform.position = new Vector3(transform.position.x, startPosition.y + bobbingOffset, transform.position.z);

        if (timer >= timeLimit)
        {
            transform.position = startPosition;

            timer = 0f;
        }




    }
}
