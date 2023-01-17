using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSelectUI : MonoBehaviour
{
    [SerializeField]
    private GameObject canvasTransform;
    [SerializeField]
    private GameObject weaponWheel;
    
    [SerializeField]
    public GameObject leftHand, rightHand;

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
            menuIsOpen = true;
        }

        if (!handController.teleportHand && openMenuL.action.inProgress)
        {
            print("Opening Menu with Left Hand");
            weaponWheel.SetActive(true);
            menuIsOpen = true;
        }
    }

    private void CloseMenu()
    {
        if (!openMenuL.action.inProgress && !openMenuR.action.inProgress)
        {
            print("Closing Menu");
            weaponWheel.SetActive(false);
            menuIsOpen = false;   
        }
    }
}
