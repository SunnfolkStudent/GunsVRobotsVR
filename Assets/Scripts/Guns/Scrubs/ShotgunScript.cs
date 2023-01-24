using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunScript : MonoBehaviour
{
    private XRFire _inputs;
    private WeaponMain weaponMain;
    [SerializeField] public GunData gunData;
    public Transform spawnPoint;
    private GunSFXnVFXManager gunSfXnVFXManager; 
    public List<Vector3> Pellets; 
    [SerializeField] private Haptics haptics;
    [SerializeField] private float shootHapticIntensity = 1f;
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

    private void OnShoot()
    {
        if (!weaponMain.canShoot())
        {
            return;
        }

        if (!_inputs.fireTrigger)
        {
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

        weaponMain.timeSinceLastShot = 0;
        //haptics.ActivateHapticRight(shootHapticIntensity, 0.1f);
    }
}
