    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;


public class BoatManager : MonoBehaviour
{

    // Boat
    public Rigidbody rb;

     // Cannons
     [SerializeField] public List<GameObject> leftSideCannons;
     [SerializeField] public List<GameObject> rightSideCannons;

     // Sails
     public enum SailStatus { Empty, OneQuarter, Half, ThreeQuarters, Full};  
     public SailStatus currentSailStatus;

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


    [Header("Ship Movement")]
    public float shipThrust = 10.0f;
    public float shipDrag = 0.5f;
    public float shipAngle = 0.0f;

    [Header("Ship Steering")]
    [SerializeField] private GameObject steeringWheel;
    public SteeringManager steeringManager;
    private float currentWheelRotation;
    public float rotationSpeed = 5.0f; // Adjust this value for a slower or faster turn
    public float maxTurnAngle = 30.0f; // Maximum turning angle per second at full wheel rotation
    private float fullSpeedTurnSpeed = 0.0f;

    [Header("Sail Objects")]
    public GameObject FullSails;
    public GameObject ThreeQSails;
    public GameObject HalfSails;
    public GameObject OneQSails;
    public GameObject EmptySails;



    public int shipHealth = 100;

    void Awake()
    {

    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        fullSpeedTurnSpeed = maxTurnAngle;

        SetSail(SailStatus.Full);

        if (steeringWheel != null)
        {
            steeringManager = steeringWheel.GetComponent<SteeringManager>();
            if (steeringManager == null)
            {
                Debug.LogError("SteeringManager script not found on the assigned SteeringWheel object.");
            }
        }
        else
        {
            Debug.LogError("SteeringWheel object is not assigned in the Inspector.");
        }

        UnityEngine.Random.InitState(Environment.TickCount);

    }

    private void Update()
    {

        if (steeringManager != null)
        {
            currentWheelRotation = steeringManager.currentWheelRotation;
        }

        //UnityEngine.Debug.Log(currentSailStatus);


    }

    private void FixedUpdate()
    {
        ApplyThrust();
        ApplyRotation();
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
        float targetTurnAngle = currentWheelRotation * maxTurnAngle;

        // Calculate the actual turn amount (smooth transition)
        float currentTurnRate = Mathf.Lerp(0, targetTurnAngle, rotationSpeed * Time.fixedDeltaTime);

        // Apply rotation using Rigidbody's torque
        Vector3 torque = Vector3.up * currentTurnRate;
        rb.AddTorque(torque, ForceMode.Acceleration);
    }
    private void ApplyDrag()
    {

    }



    // Set from UI buttons
    public void SetSail(SailStatus sailStatus)
     {
        //UnityEngine.Debug.Log("SetSail Called");
        //UnityEngine.Debug.Log(sailStatus);

        currentSailStatus = sailStatus;
        

        // This is so bad and I need to redo this but I was running out of time so don judge me please
        if (sailStatus == SailStatus.Full)
        {
            maxTurnAngle = (fullSpeedTurnSpeed - (maxTurnAngle * .0f));
            FullSails.SetActive(true);

            ThreeQSails.SetActive(false);
            HalfSails.SetActive(false);
            OneQSails.SetActive(false);
            EmptySails.SetActive(false);
        }
        if (sailStatus == SailStatus.ThreeQuarters)
        {
            maxTurnAngle = (fullSpeedTurnSpeed - (fullSpeedTurnSpeed * .10f));

            FullSails.SetActive(false);

            ThreeQSails.SetActive(true);

            HalfSails.SetActive(false);
            OneQSails.SetActive(false);
            EmptySails.SetActive(false);
        }
        if (sailStatus == SailStatus.Half)
        {
            maxTurnAngle = (fullSpeedTurnSpeed - (fullSpeedTurnSpeed * .20f));

            FullSails.SetActive(false);
            ThreeQSails.SetActive(false);

            HalfSails.SetActive(true);

            OneQSails.SetActive(false);
            EmptySails.SetActive(false);



            // Play Half Mast voice Line

        }
        if (sailStatus == SailStatus.OneQuarter)
        {
            maxTurnAngle = (fullSpeedTurnSpeed - (fullSpeedTurnSpeed * .45f));

            FullSails.SetActive(false);
            ThreeQSails.SetActive(false);
            HalfSails.SetActive(false);

            OneQSails.SetActive(true);

            EmptySails.SetActive(false);
        }
        if (sailStatus == SailStatus.Empty)
        {
            maxTurnAngle = (fullSpeedTurnSpeed - (fullSpeedTurnSpeed * .80f));

            FullSails.SetActive(false);
            ThreeQSails.SetActive(false);
            HalfSails.SetActive(false);
            OneQSails.SetActive(false);

            EmptySails.SetActive(true);


            // Play stow the sails line

            

        }

        UnityEngine.Debug.Log(maxTurnAngle);
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

    public void FireCannons(List<GameObject> cannonSide)
    {
        foreach (GameObject cannonObject in cannonSide)
        {
            CannonManager cannonManager = cannonObject.GetComponent<CannonManager>();
            if (cannonManager != null && cannonManager.canShoot)
            {
                float delay = UnityEngine.Random.Range(1f, 2f); // Delay Range of cannon shot
                cannonManager.Fire(delay);
            }
        }
    }


}
