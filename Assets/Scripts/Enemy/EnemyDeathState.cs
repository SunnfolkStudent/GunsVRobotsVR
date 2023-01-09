using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDeathState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        //Spawn X Cogs
        //    - (Health drops)
        //Spawn Y Orbs
        if (enemy.currentIntegrity <= 0)
        { 
            Instantiate(enemy.healthDrops[enemy.itemNum], enemy.transform.position, Quaternion.identity);
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
