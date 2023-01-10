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

    //sets powerups as being inactive, used for checking if we can pick up new powerup
    private void Update()
    {
        if (gunData.ShieldDisruptState <= 0 && gunData.ArmourShredState <= 0)
        {
            IsPowerUp = false;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("ShieldDisrupt") && !IsPowerUp)
        {
            
            //feeds powerup info on wether we are powered up or not
            var powerUp = col.GetComponent<PowerUpCollision>();
            powerUp.AmIPickedUp(IsPowerUp);
            
            //sets amount of shield disrupt shots we have
            gunData.ShieldDisruptState = gunData.magSize / 6;

            IsPowerUp = true;
        }
        
        //same as above but for armour shred
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
