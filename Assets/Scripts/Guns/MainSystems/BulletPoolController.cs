using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolController : MonoBehaviour
{
    public static BulletPoolController CurrentBulletPoolController;

    public List<GameObject> activePlayerBullets;
    
    public List<GameObject> inactiveEnemyBullets;
    public List<GameObject> activeEnemyBullets;
    public GameObject enemyBulletPrefab;

    private void Awake()
    {
        if (CurrentBulletPoolController)
        {
            Destroy(this);
            return;
        }

        CurrentBulletPoolController = this;
    }

    #region PLAYER_BULLETS

    public void SpawnPlayerBullet(GunData gunData, Vector3 position, Quaternion rotation)
    {
        var bullet = Instantiate(gunData.bulletPrefab, position, rotation);

        bullet.GetComponent<PlayerBulletData>().gunData = gunData;
        
        activePlayerBullets.Add(bullet);
    }

    public void DestroyPlayerBullet(PlayerBulletData playerBulletData)
    {
        activePlayerBullets.Remove(playerBulletData.gameObject);
        Destroy(playerBulletData.gameObject);
    }
    
    #endregion

    #region ENEMY_BULLETS

    public void SpawnEnemyBullet(GunData gunData, Vector3 position, Quaternion rotation)
    {
        GameObject bullet;
        if (inactiveEnemyBullets.Count != 0)
        {
            bullet = inactiveEnemyBullets[0];
            inactiveEnemyBullets.RemoveAt(0);
            bullet.transform.position = position;
            bullet.transform.rotation = rotation;
        }
        else
        {
            bullet = Instantiate(enemyBulletPrefab, position, rotation);
        }
        
        bullet.GetComponent<EnemyBulletData>().gunData = gunData;
        
        activeEnemyBullets.Add(bullet);
        bullet.SetActive(true);
    }

    public void RegisterEnemyBulletAsInactive(EnemyBulletData enemyBulletData)
    {
        enemyBulletData.gameObject.SetActive(false);
        activeEnemyBullets.Remove(enemyBulletData.gameObject);
        inactiveEnemyBullets.Add(enemyBulletData.gameObject);
    }

    #endregion
}
