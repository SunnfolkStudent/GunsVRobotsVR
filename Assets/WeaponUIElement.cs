using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUIElement : MonoBehaviour
{
    public HandController handController;
    public WeaponSelectUI weaponSelectUI;
    public WeaponMain weaponMain;
    public GameObject currentWeapon;
    public GameObject weaponHolder;


    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("Hands") && weaponSelectUI.menuIsOpen && handController.teleportHand)
        {
            weaponMain.currentGundata = currentWeapon;
            currentWeapon.transform.parent = weaponSelectUI.rightHand.transform;
        }
    }
}
