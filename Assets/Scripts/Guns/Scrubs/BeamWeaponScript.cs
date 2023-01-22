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
    
    #region BeamGunFloats

    private RaycastHit laser;

    private float baseDamageFallOff;

    private float shieldPierceFallOff;

    private float shieldDisruptFallOff;

    private float armourPierceFallOff;

    private float armourShredFallOff; 

    #endregion
    
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
            print("BeamIsFiring");
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

            Debug.Log("Beamdamage" + baseDamageFallOff);
            Debug.Log("Hit item" + laser.transform.gameObject.name);
            
            var enemy = laser.transform.gameObject.GetComponent<EnemyStateManager>();
            enemy.TakeDamage(baseDamageFallOff, armourPierceFallOff, armourShredFallOff, shieldPierceFallOff, shieldDisruptFallOff, 0f, 0f);
        }
        distance = laser.distance;

        gunData.currentAmmo --;
        gunData.ArmourShredState--;
        gunData.ShieldDisruptState--;
        gunData.knockBackState--; 

        weaponMain.timeSinceLastShot = 0;
        
    }
}
