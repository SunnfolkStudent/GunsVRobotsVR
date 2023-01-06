using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInitialiseState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
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
