using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUIElement : MonoBehaviour
{
    public HandController handController;
    public WeaponSelectUI weaponSelectUI;
    public WeaponMain weaponMain;
    public GameObject weaponHolder;
    private BoxCollider _boxCollider;
    public int currentWeapon;
    public bool swappingWeapon; 

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("Weapon1") && weaponSelectUI.menuIsOpen && handController.teleportHand)
        {
            currentWeapon = 0; 
            swappingWeapon = true; 
        }
        
        if (col.transform.CompareTag("Weapon2") && weaponSelectUI.menuIsOpen && handController.teleportHand)
        {
            currentWeapon = 1; 
            swappingWeapon = true; 
        }
        
        if (col.transform.CompareTag("Weapon3") && weaponSelectUI.menuIsOpen && handController.teleportHand)
        {
            currentWeapon = 2; 
            swappingWeapon = true; 
        }
        
        if (col.transform.CompareTag("Weapon4") && weaponSelectUI.menuIsOpen && handController.teleportHand)
        {
            currentWeapon = 3; 
            swappingWeapon = true; 
        }
        
        if (col.transform.CompareTag("Weapon5") && weaponSelectUI.menuIsOpen && handController.teleportHand)
        {
            currentWeapon = 4; 
            swappingWeapon = true; 
        }
    }
}
