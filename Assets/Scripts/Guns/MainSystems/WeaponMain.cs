using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponMain : MonoBehaviour
{
    [SerializeField] public GunData gunData;
    [SerializeField] public GameObject Hitbox_head; 

    public LayerMask laserLayer; 

    public List<GunData> GunDataMenus = new List<GunData>();

    public GameObject Bullets;

    private GunSFXnVFXManager gunSfXnVFXManager; 

    public int currentGundata { get; private set; } = 0; 

    private PowerUpManager powerUpManager;
    private LineRenderer _lineRenderer;

    public float swapTimer { get; private set; }
    private float weaponTimer; 

    private XRFire _inputs;

    private BoxCollider _boxCollider;

    private int CurrentBullet; 

    public float timeSinceLastShot;

    public Transform spawnPoint;

    public float StartTime = 0f;

     

    public bool isSwap { get; private set; } = false;

    private LineRenderer aim;
    
    #region ShotGunData
    
    private int pelletCount = 10;
    
    private int spreadAngle = 30;
    
    #endregion


    private void Start()
    {
        _inputs = GetComponentInParent<XRFire>();
        _lineRenderer = GetComponent<LineRenderer>();
        gunSfXnVFXManager = GetComponent<GunSFXnVFXManager>();
        powerUpManager = Hitbox_head.GetComponent<PowerUpManager>();

        foreach (var Weapon in GunDataMenus)
        {
            Weapon.currentAmmo = Weapon.magSize;
            Weapon.ArmourShredState = 0;
            Weapon.ShieldDisruptState = 0;
        }

        
        
    }

    private void Update()
    {
        LaserSight();
        timeSinceLastShot += Time.deltaTime;
        gunData = GunDataMenus[currentGundata]; 
        gunData.weaponStateManager();
        powerUpManager.gunData = gunData;

        gunSfXnVFXManager.currentWeapon = currentGundata; 

        if (PauseManager.IsPaused) return;
        
        reloading();
        SwapWeapon();
        updateAmmo();
        
    }

    public void ResetWeaponState()
    {
        //Reset the state of all weapons, effects and ammo counters
        foreach (var Weapon in GunDataMenus)
        {
            Weapon.currentAmmo = Weapon.magSize;
            Weapon.ArmourShredState = 0;
            Weapon.ShieldDisruptState = 0;
        }

        currentGundata = 0;
        gunData = GunDataMenus[0];

        powerUpManager.IsPowerUp = false;

        swapTimer = 0f;
        isSwap = false;
        weaponTimer = 0f;
        timeSinceLastShot = 0f;
        StartTime = 0f;
        gunData.knockBackState = 0f;
    }

    //bool that checks that we're not reloading and that we're not shooting faster than our firerate
    public bool canShoot() => !gunData.reloading && !isSwap && timeSinceLastShot > 1f / (gunData.fireRate / 60f) && (gunData.currentAmmo > 0);

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
            currentGundata = 1;
            swapTimer = Time.time;
            isSwap = true;
        }
        
        if (_inputs.swapWeapon3 && currentGundata != 2)
        {
            currentGundata = 2;
            swapTimer = Time.time;
            isSwap = true;
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

        if (_inputs.reloadPressed)
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
