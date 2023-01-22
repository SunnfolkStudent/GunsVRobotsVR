using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUIElement : MonoBehaviour
{
    public GameObject weaponHolder;
    private SphereCollider _sphereCollider;
    public int currentWeapon;
    public bool swappingWeapon; 

    private void Start()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("Weapon1"))
        {
            print("revolver");
            currentWeapon = 0; 
            swappingWeapon = true; 
        }
        
        if (col.transform.CompareTag("Weapon2"))
        {
            print("Beam");
            currentWeapon = 1; 
            swappingWeapon = true; 
        }
        
        if (col.transform.CompareTag("Weapon3"))
        {
            
            currentWeapon = 2; 
            swappingWeapon = true; 
        }
        
        if (col.transform.CompareTag("Weapon4"))
        {
            currentWeapon = 3; 
            swappingWeapon = true; 
        }
        
        if (col.transform.CompareTag("Weapon5"))
        {
            currentWeapon = 4; 
            swappingWeapon = true; 
        }
    }
}
