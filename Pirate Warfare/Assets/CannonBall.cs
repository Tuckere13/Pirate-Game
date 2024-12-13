using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public int cannonBallDamage;

    private int delay = 5;
    void Start()
    {
        Destroy(gameObject, delay);
    }


    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        // Destroy the cannonball immediately upon collision
        Destroy(gameObject);

    }


}
