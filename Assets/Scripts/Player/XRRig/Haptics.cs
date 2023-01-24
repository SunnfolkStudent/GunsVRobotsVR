using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Haptics : MonoBehaviour
{
    [SerializeField]
    private ActionBasedController rightXRController;
    [SerializeField]
    private ActionBasedController leftXRController;

    public void ActivateHapticRight(float intensity, float time)
    {
        rightXRController.SendHapticImpulse(intensity, time);
    }

    public void ActivateHapticLeft(float intensity, float time)
    {
        leftXRController.SendHapticImpulse(intensity, time);
    }
}
