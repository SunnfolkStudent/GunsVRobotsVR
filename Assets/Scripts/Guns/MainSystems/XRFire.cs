using System;
using UnityEngine;

public class XRFire : MonoBehaviour
{
    public bool fireTrigger; 
    public bool fireHeld;
    public bool reloadPressed;
    public bool reloadTrigger;

    public bool swapWeapon1;
    public bool swapWeapon2;
    public bool swapWeapon3; 

    private XRIDefaultInputActions _actions;

    private void Awake()
    {
        _actions = new XRIDefaultInputActions();
        swapWeapon1 = false;
        swapWeapon2 = false;
        swapWeapon3 = false; 
    }

    private void Update()
    {
        fireTrigger = _actions.XRIRightHandInteraction.FireButton.triggered;
        fireHeld = _actions.XRIRightHandInteraction.FireButton.inProgress;
        reloadPressed = _actions.XRIRightHandInteraction.Reload.inProgress;
        reloadTrigger = _actions.XRIRightHandInteraction.Reload.triggered;

       
    }

    private void OnEnable()
    {
        _actions.Enable();
    }

    private void OnDisable()
    {
        _actions.Disable();
    }
}
