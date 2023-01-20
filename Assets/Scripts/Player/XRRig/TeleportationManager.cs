using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


public class TeleportationManager : MonoBehaviour
{
    public GameObject BaseControllerGameObject;
    public GameObject teleportationGameObject;
    
    public InputActionReference teleportActivationReferance;

    [Space]
    public UnityEvent onTeleportActivate;
    public UnityEvent onTeleportCancel;

    private void Start()
    {
        teleportActivationReferance.action.performed += TeleportModeActivate;
        teleportActivationReferance.action.canceled += TeleportModeCancel;
    }

    private void OnDisable()
    {
        onTeleportActivate.RemoveAllListeners();
        onTeleportCancel.RemoveAllListeners();
    }


    private void TeleportModeActivate(InputAction.CallbackContext obj) => Invoke("DeactivateTeleporter", .1f);

    public void DeactivateTeleporter() => onTeleportCancel.Invoke();

    private void TeleportModeCancel(InputAction.CallbackContext obj) => onTeleportActivate.Invoke();
}