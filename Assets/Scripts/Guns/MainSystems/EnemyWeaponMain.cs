using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponMain : MonoBehaviour
{
    [SerializeField] private GunData gunData;

    public GameObject[] Bullets;

    private float timer = 0f;
    
    private int CurrentBullet; 

    private float timeSinceLastShot;

    public Transform spawnPoint;
    
    
    

    private void Start()
    {
        gunData.currentAmmo = gunData.magSize;
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;    
    }

    private bool canShoot() => timeSinceLastShot > 1f / (gunData.fireRate / 60f) && (gunData.currentAmmo > 0);

    private void shoot()
    {
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
            }
        }
    }
    
}
