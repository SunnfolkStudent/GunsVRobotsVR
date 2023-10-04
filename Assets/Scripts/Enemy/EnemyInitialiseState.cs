using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyInitialiseState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.currentShield = enemy.enemyStats.maxShield;
        enemy.currentArmour = enemy.enemyStats.maxArmour;
        enemy.currentIntegrity = enemy.enemyStats.maxIntegrity;
        
        enemy.SwitchState(enemy.InactiveState);
    }

    public override void HandleState(EnemyStateManager enemy)
    {
        
    }
}
