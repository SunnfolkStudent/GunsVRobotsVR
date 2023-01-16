using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxMove : MonoBehaviour
{
    [SerializeField] private Transform targetPosition;
    
    private void Update()
    {
        transform.position = targetPosition.position;
    }
}
