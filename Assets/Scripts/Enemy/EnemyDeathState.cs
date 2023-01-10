using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDeathState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        GameObject.Instantiate(enemy.lootDrops[enemy.itemNum], enemy.transform.position, Quaternion.identity);
        GameObject.Instantiate(enemy.healthDrops[enemy.itemNum], enemy.transform.position, Quaternion.identity);

        enemy.gameObject.SetActive(false);
        EnemyPoolController.CurrentEnemyPoolController.RegisterEnemyAsInactive(enemy);
    }

    public override void HandleState(EnemyStateManager enemy)
    {
        
    }
    
}
