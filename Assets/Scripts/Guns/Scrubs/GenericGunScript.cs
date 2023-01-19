using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericGunScript : MonoBehaviour
{
    private XRFire _inputs;
    private WeaponMain weaponMain;
    [SerializeField] public GunData gunData;
    public Transform spawnPoint;
    private GunSFXnVFXManager gunSfXnVFXManager; 
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
        if (_inputs.fireTrigger && !_inputs.reloadPressed)
        {
            if (weaponMain.canShoot())
            {
                //instantiates bullet on shot, setting direction and spawn rotation 
                BulletPoolController.CurrentBulletPoolController.SpawnPlayerBullet(gunData, transform.position,
                    spawnPoint.rotation);
                
                gunData.currentAmmo --;
                gunData.ArmourShredState--;
                gunData.ShieldDisruptState--;
                gunData.knockBackState--; 

                weaponMain.timeSinceLastShot = 0;
                
                /*gunSfXnVFXManager.onShoot();*/ 

                
                
            }
        }
    }
    
    
}
