using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponMain : MonoBehaviour
{
    [SerializeField] private GunData gunData;

    public LayerMask laserLayer; 

    public List<GunData> GunDataMenus = new List<GunData>();

    public GameObject[] Bullets;

    private int currentGundata = 0; 

    private PowerUpManager powerUpManager;
    private LineRenderer _lineRenderer;     

    private float swapTimer;
    private float weaponTimer; 

    private PlaceHolderInputs _inputs;

    private BoxCollider _boxCollider;

    private int CurrentBullet; 

    private float timeSinceLastShot;

    public Transform spawnPoint;

    public float StartTime = 0f;

    private bool isSwap = false;

    private LineRenderer aim;

    #region BeamGunFloats

    private RaycastHit laser;

    private float baseDamageFallOff;

    private float shieldPierceFallOff;

    private float shieldDisruptFallOff;

    private float armourPierceFallOff;

    private float armourShredFallOff; 

    #endregion
    

    private void Start()
    {
        _inputs = GetComponentInParent<PlaceHolderInputs>();
        powerUpManager = GetComponentInParent<PowerUpManager>();
        _lineRenderer = GetComponent<LineRenderer>();
        
        foreach (var Weapon in GunDataMenus)
        {
            Weapon.currentAmmo = gunData.magSize;
            Weapon.ArmourShredState = 0;
            Weapon.ShieldDisruptState = 0;
        }
    }

    private void Update()
    {
        shoot();
        reloading();
        timeSinceLastShot += Time.deltaTime;
        gunData = GunDataMenus[currentGundata]; 
        gunData.weaponStateManager();
        powerUpManager.gunData = gunData;
        SwapWeapon();
        updateAmmo();
        LaserSight();
    }

    //bool that checks that we're not reloading and that we're not shooting faster than our firerate
    private bool canShoot() => !gunData.reloading && !isSwap && timeSinceLastShot > 1f / (gunData.fireRate / 60f) && (gunData.currentAmmo > 0);

    private void shoot()
    {
        if (currentGundata != 1 && _inputs.FireButton && !_inputs.ReloadButton)
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

        if (currentGundata == 1 && _inputs.FireHold && !_inputs.ReloadButton)
        {
            if (!canShoot())
            {
                return;
            }
            RaycastHit laser; 
            
            if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out laser, (gunData.range * gunData.bulletSpeed), laserLayer) && laser.collider.tag == "Enemy" )
            {
                if (laser.distance >= (gunData.range / 2))
                {
                    baseDamageFallOff = (gunData.BaseDamage - (gunData.fallOff * (laser.distance - (gunData.range / 2))));
                    armourPierceFallOff = (gunData.ArmourPierce - (gunData.fallOff * (laser.distance - (gunData.range / 2))));
                    armourShredFallOff = (gunData.ArmourShred - (gunData.fallOff * (laser.distance - (gunData.range / 2))));
                    shieldPierceFallOff = (gunData.ShieldPierce - (gunData.fallOff * (laser.distance - (gunData.range / 2))));
                    shieldDisruptFallOff = (gunData.ArmourShred - (gunData.fallOff * (laser.distance - (gunData.range / 2))));
                }
                
                else

                {
                    baseDamageFallOff = gunData.BaseDamage;
                    armourPierceFallOff = gunData.ArmourPierce;
                    armourShredFallOff = gunData.ArmourShred;
                    shieldDisruptFallOff = gunData.ShieldDisrupt;
                    shieldPierceFallOff = gunData.ArmourShred;
                }
                
                var enemy = laser.transform.gameObject.GetComponent<EnemyHitdetection>();
                enemy.TakeDamage(baseDamageFallOff, armourPierceFallOff, armourShredFallOff, shieldPierceFallOff,
                    shieldDisruptFallOff);
            }
            
            gunData.currentAmmo --;
            gunData.ArmourShredState--;
            gunData.ShieldDisruptState--;

            timeSinceLastShot = 0;
        }
            
    }

    private void LaserSight()
    {
        RaycastHit laser;

        var laserPoint = (spawnPoint.forward * (gunData.range * gunData.bulletSpeed));

        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out laser, (gunData.range * gunData.bulletSpeed), laserLayer))
        {
            if (laser.collider)
            {
                _lineRenderer.SetPosition(0, spawnPoint.position);
                _lineRenderer.SetPosition(1, laser.point);
            }
        }
        
        else
        
        {
            _lineRenderer.SetPosition(0, spawnPoint.position);
            _lineRenderer.SetPosition(1, spawnPoint.position + (spawnPoint.forward * (gunData.range * gunData.bulletSpeed)));
        }
    }
    
    private void updateAmmo()
    {
        foreach (var Weapon in GunDataMenus)
        {
            if (GunDataMenus[currentGundata] != Weapon && (Weapon.currentAmmo < Weapon.magSize))
            {
                if (Time.time > (weaponTimer + Weapon.reloadTime))
                {
                    print("passiveupdate"); 
                    Weapon.currentAmmo += Weapon.reloadAmount;
                    weaponTimer = Time.time;
                    if (Weapon.currentAmmo >= Weapon.magSize)
                    {
                        Weapon.currentAmmo = Weapon.magSize;
                    }
                }
            }
        }
    }

    private void SwapWeapon()
    {
        if (_inputs.swapWeapon1 && currentGundata != 0)
        {
            currentGundata = 0;
            swapTimer = Time.time;
            isSwap = true;
        }

        if (_inputs.swapWeapon2 && currentGundata != 1)
        {
            print("I am about to swap");
            currentGundata = 1;
            swapTimer = Time.time;
            isSwap = true;
            print("I am currentlySwapping");
        }

        if (isSwap && Time.time > (swapTimer + gunData.swapTime))
        {
            print("About to end swapping");
            isSwap = false;
            weaponTimer = Time.time;
            print("I have swapped"); 
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
            
            if (Time.time > (StartTime + gunData.reloadTime))
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
