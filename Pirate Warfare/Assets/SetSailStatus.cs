using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetSailStatus : MonoBehaviour
{
    public BoatManager boatManager;
    public BoatManager.SailStatus sailStatusToSet;


    private void Awake()
    {
        // Automatically find the BoatManager in the parent or the hierarchy
        boatManager = GetComponentInParent<BoatManager>();

        if (boatManager == null)
        {
            Debug.Log("BoatManager not found on parent objects. Make sure this script is attached to a part of the ship.");
        }
    }
    public void OnButtonClick()
    {
        if (boatManager != null)
        {
            Debug.Log($"Button clicked! Setting sail status to: {sailStatusToSet}");
            boatManager.SetSail(sailStatusToSet);
        }
        else
        {
            Debug.LogWarning("BoatManager reference is not set!");
        }
    }

}
