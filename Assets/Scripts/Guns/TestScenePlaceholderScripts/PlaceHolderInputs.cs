using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaceHolderInputs : MonoBehaviour
{
    public Vector2 MoveVector { get; private set; }
    public Vector2 LookDirection { get; private set; }

    public bool FireButton { get; private set; }
    public bool ReloadButton { get; private set; }
    public bool reloadTrigger;
    public bool FireTriggered { get; private set; }
    

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
        FireButton = _placeHolderGunScene.Move.Fire.inProgress;
        FireTriggered = _placeHolderGunScene.Move.Fire.triggered;
        ReloadButton = _placeHolderGunScene.Move.Reload.inProgress;
        reloadTrigger = _placeHolderGunScene.Move.Reload.triggered;
    }
}
