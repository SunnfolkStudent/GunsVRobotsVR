using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] public GunData gunData; 
    public bool IsShieldDisrupt = false;
    public bool IsArmourShred = false;

    public BoxCollider _boxCollider;
    private Rigidbody _rigidbody;

    private float timerStart;
    private float timer = 0.1f; 

    private void Start()
    {
        _boxCollider = GetComponentInParent<BoxCollider>();
        _rigidbody = GetComponentInParent<Rigidbody>();
    }

    private void Update()
    {
        if (gunData.ShieldDisruptState <= 0)
        {
            IsShieldDisrupt = false;
        }

        if (gunData.ArmourShredState <= 0)
        {
            IsArmourShred = false; 
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("ShieldDisrupt") && !IsShieldDisrupt && !IsArmourShred)
        {
            IsShieldDisrupt = true;
            gunData.ShieldDisruptState = gunData.magSize / 6;
        }
        
        if (collision.transform.CompareTag("ArmourShred") && !IsShieldDisrupt && !IsArmourShred)
        {
            IsArmourShred = true;
            gunData.ArmourShredState = gunData.magSize / 6;
        }
    }
    
}
