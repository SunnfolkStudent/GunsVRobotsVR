using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDeathState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        EnemyPoolController.CurrentEnemyPoolController.OnEnemyKill.Invoke();

        //Spawn X Cogs
        //    - (Health drops)
        //Spawn Y Orbs
        EnemyStateManager.Instantiate(enemy.lootDrops[enemy.itemNum], enemy.transform.position, Quaternion.identity);
        EnemyStateManager.Instantiate(enemy.healthDrops[enemy.itemNum], enemy.transform.position, Quaternion.identity);

            //Spawn Particles & VFX
        //    - Despawn Enemy
        //    - Are we going to use Object Pooling?
        enemy.gameObject.SetActive(false);
        EnemyPoolController.CurrentEnemyPoolController.RegisterEnemyAsInactive(enemy);
    }

    public override void HandleState(EnemyStateManager enemy)
    {
        
    }
    
}
