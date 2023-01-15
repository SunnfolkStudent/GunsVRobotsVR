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
    public float moveAmount;
    public AnimationCurve exponential;
    public bool go;

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
        }
        
        if (position.z <= zGoal)
            return;

        if (!go)
             return;
        
        moveAmount *= exponential.Evaluate(lerpTime);
        
        position.z -= moveAmount * Time.deltaTime;
        position.z = Mathf.Clamp(position.z, zGoal, zDistance);
        
        transform.position = position;
    }
}