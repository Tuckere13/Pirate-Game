    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Unity.VisualScripting;
    using UnityEngine;


public class BoatManager : MonoBehaviour
{

    // Boat
    public Rigidbody rb;

     // Cannons
     [SerializeField] private List<GameObject> leftSideCannons;
     [SerializeField] private List<GameObject> rightSideCannons;

     // Sails
     public enum SailStatus { Empty, OneQuarter, Half, ThreeQuarters, Full};  
     public SailStatus currentSailStatus = SailStatus.Full;

    public static class SailStatusValues // Set Enums to decimal values
    {
        public static readonly Dictionary<SailStatus, float> Values = new Dictionary<SailStatus, float>
    {
        { SailStatus.Empty, 0f },
        { SailStatus.OneQuarter, 0.25f },
        { SailStatus.Half, 0.5f },
        { SailStatus.ThreeQuarters, 0.75f },
        { SailStatus.Full, 1f }
    };
    } 


    // Ship Steering


    // Ship Movement
    public float shipThrust = 10.0f;
    public float shipDrag = 0.5f;
    public float shipAngle = 0.0f;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
         // set th forward vector to the front of ship
        UnityEngine.Random.InitState(System.Environment.TickCount);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            FireCannons(leftSideCannons);
        }

        
    }

    private void FixedUpdate()
    {
        ApplyThrust();
    }

    private void ApplyThrust()
    {
        float thrustMultiplier = SailStatusValues.Values[currentSailStatus];

        // Ensure that thrustMultiplier and shipThrust have meaningful values
        if (thrustMultiplier > 0 && shipThrust > 0)
        {
            Vector3 thrustForce = -transform.right * thrustMultiplier * shipThrust;
            rb.AddForce(thrustForce, ForceMode.Acceleration);
        }
    }
    private void ApplyRotation()
    {

    }
    private void ApplyDrag()
    {

    }



    // Set from UI buttons
    void SetSail(SailStatus sailStatus)
     {
        currentSailStatus = sailStatus;
     }

    private bool CheckCannonStatus(List<GameObject> cannonSide)
    {
        foreach (GameObject cannonObject in cannonSide)
        {
            CannonManager cannonManager = cannonObject.GetComponent<CannonManager>();
            if (cannonManager == null || !cannonManager.canShoot)  // canShoot set in cannon manager
            {
                return false; // If any cannon is not reloaded, return false
            }
        }
        return true; // All cannons are reloaded and ready to shoot
    }

    private void FireCannons(List<GameObject> cannonSide)
    {
        foreach (GameObject cannonObject in cannonSide)
        {
            CannonManager cannonManager = cannonObject.GetComponent<CannonManager>();
            if (cannonManager != null && cannonManager.canShoot)
            {
                float delay = UnityEngine.Random.Range(0f, 1f);
                cannonManager.Fire(delay);
            }
        }
    }


}
