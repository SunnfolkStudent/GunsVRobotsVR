using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaceHolderInputs : MonoBehaviour
{
    public Vector2 MoveVector { get; private set; }
    public Vector2 LookDirection { get; private set; }

    public bool FireButton { get; private set; }
    public bool FireHold { get; private set;  }
    public bool ReloadButton { get; private set; }
    public bool reloadTrigger { get; private set; }
    
    public bool swapWeapon1 { get; private set; }
    
    public bool swapWeapon2 { get; private set; }
    
    public bool swapWeapon3 { get; private set; }
    

    private PlaceHolderGunScene _placeHolderGunScene;


    #region MyRegion
    private void Awake()
    {
        _placeHolderGunScene = new PlaceHolderGunScene();
    }

    private void OnEnable()
    {
        _placeHolderGunScene.Enable();
    }

    private void OnDisable()
    {
        _placeHolderGunScene.Disable();
    }

    #endregion

    private void Update()
    {
        MoveVector = _placeHolderGunScene.Move.Walk.ReadValue<Vector2>();
        LookDirection = _placeHolderGunScene.Move.Look.ReadValue<Vector2>();

        FireButton = _placeHolderGunScene.Move.Fire.triggered;
        FireHold = _placeHolderGunScene.Move.Fire.inProgress; 
        
        ReloadButton = _placeHolderGunScene.Move.Reload.inProgress;
        reloadTrigger = _placeHolderGunScene.Move.Reload.triggered;

        swapWeapon1 = _placeHolderGunScene.Move.Swap1.triggered;
        swapWeapon2 = _placeHolderGunScene.Move.swap2.triggered;
        swapWeapon3 = _placeHolderGunScene.Move.swap3.triggered; 
    }
}
