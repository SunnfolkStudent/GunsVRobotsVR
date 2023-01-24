using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamWeaponScript : MonoBehaviour
{
    private XRFire _inputs;
    private WeaponMain weaponMain;
    [SerializeField] public GunData gunData;
    public Transform spawnPoint;
    private GunSFXnVFXManager gunSfXnVFXManager;
    public float distance;

    void Start()
    {
        _inputs = GetComponentInParent<XRFire>();
        weaponMain = GetComponentInParent<WeaponMain>(); 
    }

    // Update is called once per frame
    void Update()
    {
        gunData = weaponMain.gunData;
        gunSfXnVFXManager = GetComponentInParent<GunSFXnVFXManager>(); 
        OnShoot();
    }

    private void OnShoot()
    {
        if (_inputs.fireHeld)
        {
            //gunSfXnVFXManager.BeamVFXSFXInit();
        }
        else
        {
          //  gunSfXnVFXManager.BeamVFXSFXExit();
        }
        if (_inputs.fireTrigger)
        {
            gunSfXnVFXManager.BeamVFXSFXInit();
        }

        if (_inputs.fireReleased || gunData.currentAmmo <= 0 || _inputs.reloadPressed)
        {
            gunSfXnVFXManager.BeamVFXSFXExit();
            return;
        }

        RaycastHit laser;

        if (!_inputs.fireHeld)
        {
            return; 
        }
        
        
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out laser, (gunData.range * gunData.bulletSpeed), weaponMain.laserLayer))
        {
            if (laser.transform.CompareTag("Enemy"))
            {
                if (laser.transform.TryGetComponent(out EnemyStateManager enemy))
                {
                    enemy.TakeDamage(gunData.BaseDamage, gunData.ArmourPierce, gunData.ArmourShred,
                        gunData.ShieldPierce, gunData.ShieldDisrupt, 0f, 0f);
                }
                else if (TryGetComponent(out SentryBehaviour sentry))
                {
                    sentry.TakeDamage(gunData.BaseDamage, gunData.ArmourPierce, gunData.ArmourShred,
                        gunData.ShieldPierce, gunData.ShieldDisrupt);
                }
                else if (TryGetComponent(out SentryProjectileBehaviour projectile))
                {
                    projectile.TakeDamage(gunData.BaseDamage, gunData.ArmourPierce, gunData.ArmourShred,
                        gunData.ShieldPierce, gunData.ShieldDisrupt);
                }
            }
        }
        distance = laser.distance;

        gunData.currentAmmo --;
        gunData.ArmourShredState--;
        gunData.ShieldDisruptState--;
        gunData.knockBackState--; 

        weaponMain.timeSinceLastShot = 0;
        
    }
}
