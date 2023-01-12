using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveTowardsState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        
    }

    public override void HandleState(EnemyStateManager enemy)
    {
        enemy.distanceToPlayer = CalculateDistanceToPlayer(enemy);
        //TODO: add engine reeving maybe, sparks flying?

        //If the player has moved enough, update the agent's movement target
        if (Vector3.Magnitude(enemy.destination - enemy.playerData.position) >= 1f)
        {
            enemy.destination = enemy.playerData.position;
            enemy.agent.destination = enemy.destination;
        }
        
        if (enemy.distanceToPlayer <= enemy.enemyStats.attackRange * 0.8f)
        {
            enemy.agent.ResetPath();
            enemy.SwitchState(enemy.EngageState);
        }
    }
}
