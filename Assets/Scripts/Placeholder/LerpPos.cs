using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpPos : MonoBehaviour
{

    public float ZDistance = 20000;
    public float zGoal = 309;
    public float lerpTime = 1f;
    public float moveAmount;
    public AnimationCurve _exponential;

    private void Update()
    {
        var position = transform.position;
        
        if (position.z <= zGoal)
            return;

        moveAmount *= _exponential.Evaluate(lerpTime);
        
        position.z -= moveAmount * Time.deltaTime;
        position.z = Mathf.Clamp(position.z, zGoal, ZDistance);

        transform.position = position;
    }
}
