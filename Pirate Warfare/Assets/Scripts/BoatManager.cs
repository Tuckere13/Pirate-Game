    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Unity.VisualScripting;
    using UnityEngine;

public class BoatManager : MonoBehaviour
    {

        // Cannons
        [SerializeField] private List<GameObject> leftSideCannons;
        [SerializeField] private List<GameObject> rightSideCannons;

        // Sails
        public enum SailStatus { Empty, OneQuarter, Half, ThreeQuaters, Full};  
        public SailStatus currentSailStatus = SailStatus.Empty;


    // Ship Steering


    // Ship Movement



    private void Start()
    {
        UnityEngine.Random.InitState(System.Environment.TickCount);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            FireCannons(leftSideCannons);
        }
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
