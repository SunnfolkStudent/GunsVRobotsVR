using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSelectUI : MonoBehaviour
{
    [SerializeField]
    private GameObject canvasTransform;
    [SerializeField]
    private GameObject weaponWheel;

    [SerializeField]
    private Transform faceTrack;

    public HandController handController;
    public InputActionReference openMenuL, openMenuR;
    [SerializeField]
    public bool menuIsOpen, menuIsMoving;

    private void Update()
    {
        if (!menuIsOpen)
        {
            MoveMenu();
            OpenMenu();
        }

        if (menuIsOpen)
        {
            CloseMenu();
            menuIsMoving = false;
        }
    }

    private void MoveMenu()
    {
        menuIsMoving = true;
        var position = faceTrack.position;
        var forward = new Vector3(faceTrack.forward.x, 0f, faceTrack.forward.z).normalized;
        
        canvasTransform.transform.position = position + forward * 2f;
        canvasTransform.transform.rotation = Quaternion.LookRotation(forward);
    }

    private void OpenMenu()
    {
        if (HandController.teleportHand && openMenuR.action.inProgress)
        {
            print("Opening Menu with Right Hand");
            weaponWheel.SetActive(true);
            foreach (var child in weaponWheel.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }
            menuIsOpen = true;
        }

        if (!HandController.teleportHand && openMenuL.action.inProgress)
        {
            print("Opening Menu with Left Hand");
            weaponWheel.SetActive(true);
            foreach (var child in weaponWheel.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }
            menuIsOpen = true;
        }
    }

    private void CloseMenu()
    {
        if (!HandController.teleportHand && !openMenuL.action.inProgress)
        {
            print("Closing Menu");
            weaponWheel.SetActive(false);
            foreach (var child in weaponWheel.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            }
            menuIsOpen = false;  
        }
        else if (HandController.teleportHand && !openMenuR.action.inProgress)
        {
            print("Closing Menu");
            weaponWheel.SetActive(false);
            foreach (var child in weaponWheel.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            }
            menuIsOpen = false;   
        }
    }
}
