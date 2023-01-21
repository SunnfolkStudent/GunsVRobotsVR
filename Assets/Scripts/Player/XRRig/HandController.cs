using UnityEngine;

public class HandController : MonoBehaviour
{
    [Header("Teleport Hand Configuration")]
    [Space]
    
    [Tooltip("True = LefHandTeleport, False = RightHandTeleport")]
    public static bool teleportHand = true;
    
    [Tooltip("Keep Empty in Editor")]
    public GameObject currentTeleportHand;
    [Tooltip("Keep Empty in Editor")]
    public GameObject currentWeaponHand;

    public GameObject leftHandTeleportController;
    public GameObject rightHandTeleportController;

    private void Start()
    {
        UpdateHands();
    }

    private void Update()
    {
        if (currentTeleportHand == null)
        {
            UpdateHands();
        }
    }

    private void UpdateHands() //Call on this function when player switches teleport hand
    {
        if (teleportHand)
        {
            //Set Teleport Hand to Left Hand
            leftHandTeleportController.SetActive(true);
            rightHandTeleportController.SetActive(false);
            currentTeleportHand = leftHandTeleportController;
            currentWeaponHand = rightHandTeleportController;
        }
        else
        {
            //Set Teleport Hand to Right Hand
            rightHandTeleportController.SetActive(true);
            leftHandTeleportController.SetActive(false);
            currentTeleportHand = rightHandTeleportController;
            currentWeaponHand = leftHandTeleportController;
        }
    }
}
