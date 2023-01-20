using System.Collections.Generic;

using UnityEngine;


public class WeaponMain : MonoBehaviour
{
    [SerializeField] public GunData gunData;
    [SerializeField] public GameObject Hitbox_head;

    private WeaponUIElement _weaponUIElement; 

    public LayerMask laserLayer; 

    public List<GunData> GunDataMenus = new List<GunData>();

    public GameObject Bullets;

    private GunSFXnVFXManager gunSfXnVFXManager;

    [SerializeField] private List<GameObject> Weapons; 

    public int currentGundata { get; private set; }

    private PowerUpManager powerUpManager;
    private LineRenderer _lineRenderer;


    public float swapTimer { get; private set; } = 0; 
    private float weaponTimer; 

    private XRFire _inputs;

    private BoxCollider _boxCollider;

    private int CurrentBullet; 

    public float timeSinceLastShot;

    public List<Transform> spawnPoint;

    public float StartTime = 0f;

     

    public bool isSwap { get; private set; } = false;

    private LineRenderer aim;
    
    #region ShotGunData
    
    private int pelletCount = 10;
    
    private int spreadAngle = 30;

    public bool isittrue;
    
    #endregion


    private void Start()
    {
        _inputs = GetComponentInParent<XRFire>();
        _lineRenderer = GetComponent<LineRenderer>();
        gunSfXnVFXManager = GetComponent<GunSFXnVFXManager>();
        powerUpManager = Hitbox_head.GetComponent<PowerUpManager>();
        _weaponUIElement = GetComponent<WeaponUIElement>();
        currentGundata = 0; 

        foreach (var Weapon in Weapons)
        {
            Weapon.SetActive(false);
        }

        Weapons[currentGundata].SetActive(true);

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

        if (canShoot())
        {
            isittrue = true;
        }
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
        
        foreach (var Weapon in Weapons)
        {
            Weapon.SetActive(false);
        }

        Weapons[currentGundata].SetActive(true);

        powerUpManager.IsPowerUp = false;

        swapTimer = 0f;
        isSwap = false;
        weaponTimer = 0f;
        timeSinceLastShot = 0f;
        StartTime = 0f;
        gunData.knockBackState = 0f;
    }

    //bool that checks that we're not reloading and that we're not shooting faster than our firerate
    public bool canShoot() => !PauseManager.IsPaused && !gunData.reloading && !isSwap && timeSinceLastShot > 1f / (gunData.fireRate / 60f) && (gunData.currentAmmo > 0);

    private void LaserSight()
    {
        RaycastHit laser;

        var laserPoint = (spawnPoint[currentGundata].forward * (gunData.range * gunData.bulletSpeed));

        if (Physics.Raycast(spawnPoint[currentGundata].position, spawnPoint[currentGundata].forward, out laser, (gunData.range * gunData.bulletSpeed), laserLayer))
        {
            if (laser.collider)
            {
                _lineRenderer.SetPosition(0, spawnPoint[currentGundata].position);
                _lineRenderer.SetPosition(1, laser.point);
            }
        }
        
        else
        
        {
            _lineRenderer.SetPosition(0, spawnPoint[currentGundata].position);
            _lineRenderer.SetPosition(1, spawnPoint[currentGundata].position + (spawnPoint[currentGundata].forward * (gunData.range * gunData.bulletSpeed)));
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
        if (_weaponUIElement.swappingWeapon == true)
        {
            Weapons[currentGundata].SetActive(false);

            currentGundata = _weaponUIElement.currentWeapon;

            Weapons[currentGundata].SetActive(true);
            
            swapTimer = Time.time;
            isSwap = true;

            _weaponUIElement.swappingWeapon = false; 
            
            
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
            print("Pressed;");
        }

        if (_inputs.reloadPressed)
        {
            print("button pressed");
            if (gunData.currentAmmo >= gunData.magSize)
            {
                print("I am full");
                return;
            }
            
            if (Time.time > (StartTime + gunData.reloadTime))
            {
                //increases ammo by the amount defined in gunData.reloadAmount on a timer defined in the reloadTime variable
                print("reloading");
                gunData.currentAmmo += gunData.reloadAmount;
                StartTime = Time.time;
                print("reloaded");
                if (gunData.currentAmmo >= gunData.magSize)
                {
                    gunData.currentAmmo = gunData.magSize;
                }
            }
        }
    }

}
