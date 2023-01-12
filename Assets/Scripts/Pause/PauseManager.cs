using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static bool IsPaused;

    private XRIDefaultInputActions _inputs;

    private void Awake()
    {
        _inputs = new XRIDefaultInputActions();
    }

    private void OnEnable()
    {
        _inputs.Enable();
    }

    private void OnDisable()
    {
        _inputs.Disable();
    }

    public void Update()
    {
        if (_inputs.XRIUI.Pause.triggered)
        {
            IsPaused = !IsPaused;
        }
    }
}
