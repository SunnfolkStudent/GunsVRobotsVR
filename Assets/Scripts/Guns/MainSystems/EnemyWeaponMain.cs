using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponMain : MonoBehaviour
{
    [SerializeField] private GunData gunData;

    public GameObject[] Bullets;

    //private float timer = 0f;
    
    private int CurrentBullet; 

    private float timeSinceLastShot;

    public Transform spawnPoint;

    public void shoot(EnemyStateManager enemy)
    {
        var randomAimOffset = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        var directionTowardsPlayer = (enemy.playerData.position - transform.position).normalized;
        var fireDirection = Quaternion.LookRotation((directionTowardsPlayer + randomAimOffset).normalized, Vector3.up);
        BulletPoolController.CurrentBulletPoolController.SpawnEnemyBullet(gunData, transform.position, fireDirection);
    }

}