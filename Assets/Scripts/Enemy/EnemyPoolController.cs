using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class EnemyPoolController : MonoBehaviour
{
    public static EnemyPoolController CurrentEnemyPoolController;
    
    public List<GameObject> activeEnemies;
    public GameObject explosionPrefab;

    private void Awake()
    {
        if (CurrentEnemyPoolController)
        {
            Destroy(this);
            return;
        }

        CurrentEnemyPoolController = this;
    }
    
    public GameObject SpawnEnemy(GameObject enemyPrefab, Vector3 position, Quaternion rotation)
    {
        var enemy = Instantiate(enemyPrefab, position, rotation);
        activeEnemies.Add(enemy);
        return enemy;
    }

    public void DestroyEnemy(GameObject enemyToDestroy)
    {
        activeEnemies.Remove(enemyToDestroy);
        var explosion = Instantiate(explosionPrefab, enemyToDestroy.transform.position, Quaternion.identity);
        if (enemyToDestroy.TryGetComponent<EnemyStateManager>(out var enemyStateManager))
        {
            explosion.transform.localScale = new Vector3(2f, 2f, 2f);
        }
        Destroy(enemyToDestroy);
    }
}
