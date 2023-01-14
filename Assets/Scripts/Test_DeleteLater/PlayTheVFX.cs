using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class PlayTheVFX : MonoBehaviour
{
    public VisualEffect _effect;

    private void Start()
    {
        _effect.SendEvent("OnStop");
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _effect.SendEvent("OnPlay");
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _effect.Reinit();
            _effect.SendEvent("OnStop");
        }
    }
}
