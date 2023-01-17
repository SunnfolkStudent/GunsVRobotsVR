using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class LerpPos : MonoBehaviour
{

    public float zDistance = 20000;
    public float zGoal = 309;
    public float lerpTime = 1f;
    public AnimationCurve curve;
    public bool go;
    private float time;
    private Vector3 position;

    private void Start()
    {
        position = transform.position;
    }

    private void Update()
    {
        if (go && position.z >= zGoal)
        {

            time += Time.deltaTime;

            float exp = curve.Evaluate(time / lerpTime);
            float moveAmount = exp * (zDistance - zGoal);

            position.z = zDistance - moveAmount;
            position.z = Mathf.Clamp(position.z, zGoal, zDistance);

            transform.position = position;
        }
    }
    public void Spawn()
    {
        go = true;
    }
    public void ResetPos()
    {
        position.z = zDistance;
        go = false;
        transform.position = position;
        time = 0;
    }
}