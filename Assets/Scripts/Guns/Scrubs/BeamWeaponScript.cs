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
        if (!weaponMain.canShoot())
        {
                return;
        }
        RaycastHit laser; 
            
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out laser, (gunData.range * gunData.bulletSpeed), weaponMain.laserLayer) && laser.collider.tag == "Enemy" )
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
            enemy.TakeDamage(baseDamageFallOff, armourPierceFallOff, armourShredFallOff, shieldPierceFallOff, shieldDisruptFallOff, 0f, 0f);
            }
            
            gunData.currentAmmo --;
            gunData.ArmourShredState--;
            gunData.ShieldDisruptState--;
            gunData.knockBackState--; 

            weaponMain.timeSinceLastShot = 0;
        
    }
}
