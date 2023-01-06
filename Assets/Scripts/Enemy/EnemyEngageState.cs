using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEngageState : EnemyBaseState
{
    public float attackDelay = 1f;
    public float attackTimer;
    
    public override void EnterState(EnemyStateManager enemy)
    {
        
    }

    public override void HandleState(EnemyStateManager enemy)
    {
        enemy.distanceToPlayer = CalculateDistanceToPlayer(enemy);
        
        //Checks if a raycast towards the player hits any environment object.
        var directionTowardsPlayer = enemy.playerData.position - enemy.transform.position;
        var isSightLineBlocked = Physics.Raycast(enemy.transform.position, directionTowardsPlayer,
            enemy.distanceToPlayer, enemy.whatIsEnvironment);

        if (enemy.distanceToPlayer > enemy.attackRange)
        {
            enemy.SwitchState(enemy.MoveTowardsState);
            return;
        }
        
        if (!isSightLineBlocked)
        {
            enemy.SwitchState(enemy.PerformAttackState);
            attackTimer = Time.deltaTime;
            return;
        }

        if (Time.deltaTime > attackTimer + attackDelay && /*Is being attacked*/)
        {
            enemy.SwitchState(enemy.EvadeState);
            return;
        }

        //Move in Circle around player between **PerformAttack**
        _destination = _agent.
    }
}
