using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class TeleportationManager : MonoBehaviour
{
    public GameObject BaseControllerGameObject;
    public GameObject teleportationGameObject;

    public InputActionReference teleportActivationReferance;

    public UnityEvent onTeleportActivate;
    public UnityEvent onTeleportCancel;

    private void Start()
    {
        teleportActivationReferance.action.performed += TeleportModeActivate;
        teleportActivationReferance.action.canceled += TeleportModeCancel;
    }

    private void TeleportModeCancel(InputAction.CallbackContext obj)
    {
        
    }

    private void TeleportModeActivate(InputAction.CallbackContext obj) => onTeleportActivate.Invoke();
}
