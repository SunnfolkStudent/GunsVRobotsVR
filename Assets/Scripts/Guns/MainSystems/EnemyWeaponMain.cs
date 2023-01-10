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

    public void shoot()
    {
        var clone = Instantiate(Bullets[0], transform.position, spawnPoint.rotation);
        var BulletData = clone.GetComponent<EnemyBulletData>();
        BulletData.gunData = gunData;
        
        Destroy(clone, gunData.range);
    }

}
