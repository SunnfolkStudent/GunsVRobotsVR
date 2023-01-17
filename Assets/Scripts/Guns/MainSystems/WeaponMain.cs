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

    public List<Vector3> Pellets; 

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

    private float timeSinceLastShot;

    public Transform spawnPoint;

    public float StartTime = 0f;

     

    public bool isSwap { get; private set; } = false;

    private LineRenderer aim;
    
    #region ShotGunData
    
    private int pelletCount = 10;
    
    private int spreadAngle = 30;
    
    #endregion

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

        Pellets = new List<Vector3>();
        Pellets.Add(new Vector3(1f,0f,0f));
        Pellets.Add(new Vector3(0.86602540378444f, 0.5f, 0f));
        Pellets.Add(new Vector3(0.86602540378444f, -0.5f, 0f));
        Pellets.Add(new Vector3(0.86602540378444f, -0.5f, 0f));
        Pellets.Add(new Vector3(0.86602540378444f, -0.5f, 0f));
        Pellets.Add(new Vector3(0.866025403f, 0.3535533906f, 0.3535533906f));
        Pellets.Add(new Vector3(0.866025403f, -0.3535533906f, 0.3535533906f));
        Pellets.Add(new Vector3(0.866025403f, 0.3535533906f, -0.3535533906f));
        Pellets.Add(new Vector3(0.866025403f, -0.3535533906f, -0.3535533906f));
        
    }

    private void Update()
    {
        LaserSight();
        timeSinceLastShot += Time.deltaTime;
        gunData = GunDataMenus[currentGundata]; 
        gunData.weaponStateManager();
        //powerUpManager.gunData = gunData;

        gunSfXnVFXManager.currentWeapon = currentGundata; 

        if (PauseManager.IsPaused) return;

        shoot();
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
    private bool canShoot() => !gunData.reloading && !isSwap && timeSinceLastShot > 1f / (gunData.fireRate / 60f) && (gunData.currentAmmo > 0);

    private void shoot()
    {
        if (currentGundata != 1 && currentGundata != 2 && _inputs.fireTrigger && !_inputs.reloadPressed)
        {
            if (canShoot())
            {
                //instantiates bullet on shot, setting direction and spawn rotation 
                BulletPoolController.CurrentBulletPoolController.SpawnPlayerBullet(gunData, transform.position,
                    spawnPoint.rotation);
                
                gunSfXnVFXManager.onShoot(); 

                gunData.currentAmmo --;
                gunData.ArmourShredState--;
                gunData.ShieldDisruptState--;
                gunData.knockBackState--; 

                timeSinceLastShot = 0;
                
            }
        }

        if (currentGundata == 1 && _inputs.fireHeld && !_inputs.reloadPressed)
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
                
                gunSfXnVFXManager.onShoot(); 
                
                var enemy = laser.transform.gameObject.GetComponent<EnemyStateManager>();
                enemy.TakeDamage(baseDamageFallOff, armourPierceFallOff, armourShredFallOff, shieldPierceFallOff,
                    shieldDisruptFallOff, 0f, 0f);
            }
            
            gunData.currentAmmo --;
            gunData.ArmourShredState--;
            gunData.ShieldDisruptState--;
            gunData.knockBackState--; 

            timeSinceLastShot = 0;
        }

        if (currentGundata == 2 && _inputs.fireTrigger && !_inputs.reloadPressed)
        {
            if (!canShoot())
            {
                print("cannot shoot");
                return;
            }

            print("can shoot");

            foreach (var pellet in Pellets)
            {
                var rotation = spawnPoint.rotation * Quaternion.FromToRotation(new Vector3(1f, 0f, 0f), pellet);
                BulletPoolController.CurrentBulletPoolController.SpawnPlayerBullet(gunData, spawnPoint.position, rotation);
            }
            
            gunSfXnVFXManager.onShoot();
            
            gunData.currentAmmo --;
            gunData.ArmourShredState--;
            gunData.ShieldDisruptState--;
            gunData.knockBackState--; 

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
