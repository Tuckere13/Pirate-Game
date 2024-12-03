using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCannons : MonoBehaviour
{
    public BoatManager boatManager;

    public enum CannonSide
    {
        Left,
        Right
    }

    [Tooltip("Select which side this script controls.")]
    public CannonSide cannonSide;

    private void Awake()
    {
        // Automatically find the BoatManager in the parent or the hierarchy
        boatManager = GetComponentInParent<BoatManager>();

        if (boatManager == null)
        {
            Debug.LogError("BoatManager not found on parent objects. Make sure this script is attached to a part of the ship.");
        }
    }

    public void OnButtonClick()
    {
        if (boatManager != null)
        {
            if (cannonSide == CannonSide.Left)
            {
                boatManager.FireCannons(boatManager.leftSideCannons);
            }
            else if (cannonSide == CannonSide.Right)
            {
                boatManager.FireCannons(boatManager.rightSideCannons);
            }
        }
        else
        {
            Debug.LogWarning("BoatManager reference is not set!");
        }
    }
}
