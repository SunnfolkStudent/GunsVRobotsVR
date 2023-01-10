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
        
        enemy.agent = enemy.GetComponent<NavMeshAgent>();
        
        enemy.agent.speed = enemy.enemyStats.moveSpeed;
        
        enemy.distanceToPlayer = CalculateDistanceToPlayer(enemy);
        
        //Set movement destination
        enemy.destination = enemy.playerData.position;
        enemy.agent.destination = enemy.destination;

        enemy.SwitchState(enemy.MoveTowardsState);
    }

    public override void HandleState(EnemyStateManager enemy)
    {
        
    }
}