using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyDeathState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        
        int lootIndexToDrop = Random.Range(0, enemy.lootDrops.Length);
        GameObject.Instantiate(enemy.lootDrops[lootIndexToDrop], enemy.transform.position, Quaternion.identity);
        
        int healthIndexToDrop = Random.Range(0, enemy.healthDrops.Length);
        GameObject.Instantiate(enemy.healthDrops[healthIndexToDrop], enemy.transform.position, Quaternion.identity);

        enemy.gameObject.SetActive(false);
        EnemyPoolController.CurrentEnemyPoolController.RegisterEnemyAsInactive(enemy);
    }

    public override void HandleState(EnemyStateManager enemy)
    {
        
    }
    
}
