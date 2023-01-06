using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WeaponMain : MonoBehaviour
{
    [SerializeField] private GunData gunData;

    public GameObject[] Bullets;

    private float timer = 0f; 

    private PlaceHolderInputs _inputs;

    private int CurrentBullet; 

    private float timeSinceLastShot;

    public Transform spawnPoint;

    public float StartTime = 0f; 
    
    

    private void Start()
    {
        _inputs = GetComponentInParent<PlaceHolderInputs>();
        gunData.currentAmmo = 10;
    }

    private void Update()
    {
        shoot();
        reloading();
        timeSinceLastShot += Time.deltaTime;    
    }

    private bool canShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f) && (gunData.currentAmmo > 0);

    private void shoot()
    {
        if (_inputs.FireButton)
        {
            if (canShoot())
            {
                print("i am shoot"); 
                var clone = Instantiate(Bullets[0], transform.position, spawnPoint.rotation);
                var BulletData = clone.GetComponent<BulletData>();
                BulletData.gunData = gunData;
                
                
                Destroy(clone, gunData.range);

                gunData.currentAmmo --;

                timeSinceLastShot = 0;

                //Debug.Break();

            }
        }
    }

    private void reloading()
    {
        if (_inputs.reloadTrigger)
        {
            StartTime = Time.time;

            print("timer works");
        }

        if (_inputs.ReloadButton)
        {
            if (Time.time > (StartTime + gunData.reloadTime))
            {
                gunData.currentAmmo = gunData.magSize;
            }
        }
        
        
    }
}
