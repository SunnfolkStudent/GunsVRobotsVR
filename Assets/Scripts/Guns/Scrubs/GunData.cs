using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Gun", menuName = "Weapon/Gun")]

public class GunData : ScriptableObject
{
    public float range;
    public float fireRate;
    public float bulletSpeed;
    public float reloadTime; 
    
    public int currentAmmo;
    public int magSize;

    public bool reloading;
    
    
    public int BaseDamage = 5;
    public int ArmourPierce = 0;
    public int ArmourShred = 0;
    public int ShieldPierce = 0;
    public int ShieldDisrupt = 0;
    public int KnockBackPush = 0;
    public int KnockBackStun = 0;
}
