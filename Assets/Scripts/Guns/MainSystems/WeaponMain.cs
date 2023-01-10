using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WeaponMain : MonoBehaviour
{
    [SerializeField] private GunData gunData;

    public GameObject[] Bullets;

    private PowerUpManager powerUpManager;

    private float reloadTime = 0.2f;

    private PlaceHolderInputs _inputs;

    private BoxCollider _boxCollider;

    private int CurrentBullet; 

    private float timeSinceLastShot;

    public Transform spawnPoint;

    public float StartTime = 0f; 
    
    

    private void Start()
    {
        _inputs = GetComponentInParent<PlaceHolderInputs>();
        gunData.currentAmmo = gunData.magSize;
        powerUpManager = GetComponentInParent<PowerUpManager>();
        gunData.ArmourShredState = 0;
        gunData.ShieldDisruptState = 0; 
    }

    private void Update()
    {
        shoot();
        reloading();
        timeSinceLastShot += Time.deltaTime;
        gunData.weaponStateManager();
        powerUpManager.gunData = gunData;
    }

    //bool that checks that we're not reloading and that we're not shooting faster than our firerate
    private bool canShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f) && (gunData.currentAmmo > 0);

    private void shoot()
    {
        if (_inputs.FireButton && !_inputs.ReloadButton)
        {
            if (canShoot())
            {
                //instantiates bullet on shot, setting direction and spawn rotation 
                var clone = Instantiate(Bullets[0], transform.position, spawnPoint.rotation);
                var BulletData = clone.GetComponent<PlayerBulletData>();
                // sets the bullet's gundata component to be the same as this script's 
                BulletData.gunData = gunData;
                
                //destroys bullet when it is out of its range
                Destroy(clone, gunData.range);
                gunData.currentAmmo --;
                gunData.ArmourShredState--;
                gunData.ShieldDisruptState--;

                timeSinceLastShot = 0;
                
            }
        }
    }

    private void reloading()
    {
        if (_inputs.reloadTrigger)
        {
            StartTime = Time.time;
        }

        if (_inputs.ReloadButton)
        {
            if (gunData.currentAmmo >= gunData.magSize)
            {
                return;
            }
            
            if (Time.time > (StartTime + reloadTime))
            {
                //increases ammo by the amount defined in gunData.reloadAmount on a timer defined in the reloadTime variable
                gunData.currentAmmo += gunData.reloadAmount;
                StartTime = Time.time;
                if (gunData.currentAmmo >= gunData.magSize)
                {
                    gunData.currentAmmo = gunData.magSize;
                }
            }
        }
    }
}
