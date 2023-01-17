using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LerpPos : MonoBehaviour
{

    public float zDistance = 20000;
    public float zGoal = 309;
    public float lerpTime = 1f;
    public AnimationCurve exponential;
    public bool go;
    private float time;

    private void Update()
    {
        var position = transform.position;
        
        
        
        if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            go = true;
        }

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            position.z = zDistance;
            go = false;
            transform.position = position;
            time = 0;
        }
        
        if (position.z <= zGoal)
            return;

        if (!go)
             return;

        time += Time.deltaTime;

        float exp = exponential.Evaluate(time / lerpTime);
        float moveAmount = exp * (zDistance - zGoal);
        
        position.z = zDistance - moveAmount;
        position.z = Mathf.Clamp(position.z, zGoal, zDistance);
        
        transform.position = position;
    }
}