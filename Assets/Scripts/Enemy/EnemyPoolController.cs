using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyPoolController : MonoBehaviour
{
    public static EnemyPoolController CurrentEnemyPoolController;
    
    public List<GameObject> inactiveEnemies;
    public List<GameObject> activeEnemies;
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
        EnemyStateManager stateManager;
        GameObject enemy;
        if (inactiveEnemies.Count != 0)
        {
            enemy = inactiveEnemies[0];
            inactiveEnemies.RemoveAt(0);
            stateManager = enemy.GetComponent<EnemyStateManager>();
        }
        else
        {
            enemy = Instantiate(enemyPrefab);
            stateManager = enemy.GetComponent<EnemyStateManager>();
        }
        
        activeEnemies.Add(enemy);

        enemy.transform.position = position;
        enemy.transform.rotation = rotation;
        
        stateManager.enemyStats = enemyStats;
        stateManager.currentState = stateManager.InitialiseState;
        stateManager.currentState.EnterState(stateManager);
        enemy.SetActive(true);
    }

    public void RegisterEnemyAsInactive(EnemyStateManager enemyStateManager)
    {
        activeEnemies.Remove(enemyStateManager.gameObject);
        inactiveEnemies.Add(enemyStateManager.gameObject);
    }
}
