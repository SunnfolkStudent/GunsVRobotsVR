using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


public class TeleportationManager : MonoBehaviour
{
    public ScreenFade screenFade;
    
    public GameObject BaseControllerGameObject;
    public GameObject teleportationGameObject;

    public InputActionReference teleportActivationReferance;
    public InputActionReference teleportCommitReferance;

    [Space]
    public UnityEvent onTeleportActivate;
    public UnityEvent onTeleportCancel;

    private void Start()
    {
        teleportActivationReferance.action.performed += TeleportModeActivate;
        teleportActivationReferance.action.canceled += TeleportModeCancel;
    }

    private void Update()
    {
        if (teleportCommitReferance.action.WasPressedThisFrame())
        {
            StartCoroutine(TeleportFade());
        }
    }

    private void TeleportModeActivate(InputAction.CallbackContext obj) => Invoke("DeactivateTeleporter", .1f);

    void DeactivateTeleporter() => onTeleportCancel.Invoke();

    private void TeleportModeCancel(InputAction.CallbackContext obj) => onTeleportActivate.Invoke();

    IEnumerator TeleportFade()
    {
        screenFade.FadeOut();
        yield return new WaitForSeconds(screenFade.fadeDuration);
        screenFade.FadeIn();
        
    }
}