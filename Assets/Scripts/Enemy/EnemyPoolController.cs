using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyPoolController : MonoBehaviour
{
    public static EnemyPoolController CurrentEnemyPoolController;
    
    public List<GameObject> inactiveEnemies;
    public GameObject enemyPrefab;
    public UnityEvent OnEnemyHit;
    public UnityEvent OnEnemyKill;

    private void Awake()
    {
        if (CurrentEnemyPoolController)
        {
            Destroy(this);
            return;
        }

        CurrentEnemyPoolController = this;
    }

    public void SpawnEnemy(EnemyStats enemyStats, Vector3 position, Quaternion rotation)
    {
        GameObject enemy;
        if (inactiveEnemies.Count != 0)
        {
            enemy = inactiveEnemies[0];
            inactiveEnemies.RemoveAt(0);
        }
        else
        {
            enemy = Instantiate(enemyPrefab);
        }

        enemy.transform.position = position;
        enemy.transform.rotation = rotation;
        var stateManager = enemy.GetComponent<EnemyStateManager>();
        
        stateManager.enemyStats = enemyStats;
        stateManager.SwitchState(stateManager.InitialiseState);
        enemy.SetActive(true);
    }

    public void RegisterEnemyAsInactive(EnemyStateManager enemyStateManager)
    {
        inactiveEnemies.Add(enemyStateManager.gameObject);
    }
}
