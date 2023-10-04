using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyInactiveState : EnemyBaseState
{
    private float _startTime;
    
    public override void EnterState(EnemyStateManager enemy)
    {
        _startTime = Time.time;
    }

    public override void HandleState(EnemyStateManager enemy)
    {
        if (Time.time < _startTime + enemy.spawnDuration) return;

        enemy.agent = enemy.GetComponent<NavMeshAgent>();

        enemy.agent.speed = enemy.enemyStats.moveSpeed;

        enemy.distanceToPlayer = CalculateDistanceToPlayer(enemy);

        //Set movement destination
        enemy.destination = enemy.playerData.position;
        enemy.agent.destination = enemy.destination;

        enemy.SwitchState(enemy.MoveTowardsState);
    }
}
