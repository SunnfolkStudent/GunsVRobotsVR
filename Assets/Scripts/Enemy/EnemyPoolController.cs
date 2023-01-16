using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyPoolController : MonoBehaviour
{
    public static EnemyPoolController CurrentEnemyPoolController;
    
    public List<GameObject> activeEnemies;

    private void Awake()
    {
        if (CurrentEnemyPoolController)
        {
            Destroy(this);
            return;
        }

        CurrentEnemyPoolController = this;
    }
    
    public void SpawnEnemy(GameObject enemyPrefab, Vector3 position, Quaternion rotation)
    {
        var enemy = Instantiate(enemyPrefab, position, rotation);
        activeEnemies.Add(enemy);
    }

    public void DestroyEnemy(GameObject enemyToDestroy)
    {
        activeEnemies.Remove(enemyToDestroy);
        Destroy(enemyToDestroy);
    }
}
