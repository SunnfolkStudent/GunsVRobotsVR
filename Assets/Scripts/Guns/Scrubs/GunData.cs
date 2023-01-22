using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu (fileName = "Gun", menuName = "Weapon/Gun")]

public class GunData : ScriptableObject
{
    public float range;
    public float fireRate;
    public float bulletSpeed;
    public float fallOff; 
    public int reloadAmount;
    public float reloadTime;
    public bool isKnockBack;

    public int currentAmmo;
    public int magSize;

    public bool reloading;
    public float swapTime; 

    public float ArmourShredState;
    public float ShieldDisruptState;
    public float knockBackState;
    
    public int BaseDamage = 0;
    public int ArmourPierce = 0;
    public int ArmourShred = 0;
    public int ShieldPierce = 0;
    public int ShieldDisrupt = 0;
    
    public float KnockBackPush = 0;
    public float KnockBackStun = 0;

    void update()
    {
        //sets how many armourshred shots you have
        var ArmourPierceRounder = (float)Math.Round(ArmourShredState, 0, MidpointRounding.AwayFromZero);

        ArmourShredState = ArmourPierceRounder;

        
        //same as above but for shieldpierce
        var ShieldPierceRounder = (float)Math.Round(ShieldDisruptState, 0, MidpointRounding.AwayFromZero);

        ArmourShredState = ArmourPierceRounder; 
    }
    
    //activates and deactivates armour and shield disrupt damage from powerups
    public void weaponStateManager()
    {
        if (ShieldDisruptState > 0)
        {
            ShieldDisrupt = BaseDamage;
        }
        
        else if (ShieldDisruptState <= 0)
        {
            ShieldDisrupt = (int)0.0;
            ShieldDisruptState = 0; 
        }

        if (ArmourShredState > 0)
        {
            ArmourShred = BaseDamage;
        }
        
        else if (ArmourShredState <= 0)
        {
            ArmourShred = (int)0.0;
            ArmourShredState = 0; 
        }
        
        if (knockBackState > 0)
        {
            isKnockBack = true; 
        }
        
        else if (knockBackState <= 0)
        {
            knockBackState = 0;
            isKnockBack = false;
        }
    }
}
