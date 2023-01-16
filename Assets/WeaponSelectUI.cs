using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSelectUI : MonoBehaviour
{
    [SerializeField]
    private GameObject canvasTransform;
    [SerializeField]
    private GameObject weaponWheel;
    
    [SerializeField]
    private GameObject leftHand, rightHand;

    public HandController handController;
    public InputActionReference openMenuL, openMenuR;
    [SerializeField]
    private bool _menuIsOpen, _menuIsMoving;

    private void Update()
    {
        if (!_menuIsOpen)
        {
            MoveMenu();
            OpenMenu();
        }

        if (_menuIsOpen)
        {
            CloseMenu();
            _menuIsMoving = false;
        }
    }

    private void MoveMenu()
    {
        _menuIsMoving = true;
        if (handController.teleportHand)
        {
            canvasTransform.transform.position = rightHand.transform.position;
            canvasTransform.transform.rotation = rightHand.transform.rotation;
        }
        else if (!handController.teleportHand)
        {
            canvasTransform.transform.position = leftHand.transform.position;
            canvasTransform.transform.rotation = leftHand.transform.rotation;
        }
    }

    private void OpenMenu()
    {
        if (handController.teleportHand && openMenuR.action.inProgress)
        {
            print("Opening Menu with Right Hand");
            weaponWheel.SetActive(true);
            _menuIsOpen = true;
        }

        if (!handController.teleportHand && openMenuL.action.inProgress)
        {
            print("Opening Menu with Left Hand");
            weaponWheel.SetActive(true);
            _menuIsOpen = true;
        }
    }

    private void CloseMenu()
    {
        if (!openMenuL.action.inProgress && !openMenuR.action.inProgress)
        {
            print("Closing Menu");
            weaponWheel.SetActive(false);
            _menuIsOpen = false;   
        }
    }
}
