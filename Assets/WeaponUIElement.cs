using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUIElement : MonoBehaviour
{
    public WeaponSelectUI weaponSelectUI;
    public SphereCollider _sphereCollider;
    public int currentWeapon;
    public bool swappingWeapon; 

    private void Start()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("Weapon1") && weaponSelectUI.menuIsOpen /*&& HandController.teleportHand*/)
        {
            print("Weapon0");
            currentWeapon = 0; 
            swappingWeapon = true; 
        }
        
        if (col.transform.CompareTag("Weapon2") && weaponSelectUI.menuIsOpen /*&& HandController.teleportHand*/)
        {
            print("Weapon1");
            currentWeapon = 1; 
            swappingWeapon = true; 
        }
        
        if (col.transform.CompareTag("Weapon3") && weaponSelectUI.menuIsOpen /*&& HandController.teleportHand*/)
        {
            print("Weapon2");
            currentWeapon = 2; 
            swappingWeapon = true; 
        }
        
        if (col.transform.CompareTag("Weapon4") && weaponSelectUI.menuIsOpen /*&& HandController.teleportHand*/)
        {
            print("Weapon3");
            currentWeapon = 3; 
            swappingWeapon = true; 
        }
        
        if (col.transform.CompareTag("Weapon5") && weaponSelectUI.menuIsOpen /*&& HandController.teleportHand*/)
        {
            print("Weapon4");
            currentWeapon = 4; 
            swappingWeapon = true; 
        }
    }
}
