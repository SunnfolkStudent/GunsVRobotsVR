using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class TeleportationManager : MonoBehaviour
{
    public GameObject BaseControllerGameObject;
    public GameObject teleportationGameObject;
    
    public InputActionReference teleportActivationReferance;
    public InputActionReference onTeleport;
    public Image image;

    [Space]
    public UnityEvent onTeleportActivate;
    public UnityEvent onTeleportCancel;

    private void Start()
    {
        teleportActivationReferance.action.performed += TeleportModeActivate;
        teleportActivationReferance.action.canceled += TeleportModeCancel;
        onTeleport.action.performed += Darktime;
        image = GetComponent<Color>();
        var color = image
    }

    private void Darktime(InputAction.CallbackContext obj)
    {
        
    }

    private void TeleportModeActivate(InputAction.CallbackContext obj) => Invoke("DeactivateTeleporter", .1f);

    void DeactivateTeleporter() => onTeleportCancel.Invoke();

    private void TeleportModeCancel(InputAction.CallbackContext obj) => onTeleportActivate.Invoke();
}