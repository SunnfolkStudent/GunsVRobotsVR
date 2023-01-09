using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] public GunData gunData;
    public bool IsPowerUp; 

    private BoxCollider _boxCollider;
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
            IsPowerUp = false;
        }

        if (gunData.ArmourShredState <= 0)
        {
            IsPowerUp = false; 
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("ShieldDisrupt") && !IsPowerUp)
        {
            gunData.ShieldDisruptState = gunData.magSize / 6;
            
            var powerUp = col.GetComponent<PowerUpCollision>();
            powerUp.AmIPickedUp(IsPowerUp);
            
            IsPowerUp = true;
            
            print("Works");
        }
        
        if (col.CompareTag("ArmourShred") && !IsPowerUp)
        {
            gunData.ArmourShredState = gunData.magSize / 6;
            
            var powerUp = col.GetComponent<PowerUpCollision>();
            powerUp.AmIPickedUp(IsPowerUp);
            
            IsPowerUp = true;
            
            print("also works");
        }
    }
}
