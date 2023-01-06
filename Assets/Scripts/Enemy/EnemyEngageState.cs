using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEngageState : EnemyBaseState
{
    public float attackDelay = 1f;
    public float attackTimer;
    
    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.agent.ResetPath();
    }

    public override void HandleState(EnemyStateManager enemy)
    {
        enemy.distanceToPlayer = CalculateDistanceToPlayer(enemy);
        
        //Checks if a raycast towards the player hits any environment object.
        var directionTowardsPlayer = enemy.playerData.position - enemy.transform.position;
        var isSightLineBlocked = Physics.Raycast(enemy.transform.position, directionTowardsPlayer,
            enemy.distanceToPlayer, enemy.whatIsEnvironment);

        if (enemy.distanceToPlayer > enemy.enemyStats.attackRange * 1.2f)
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
        if (Time.deltaTime > attackTimer + attackDelay /*Is being attacked*/)
        {
            enemy.SwitchState(enemy.EvadeState);
            return;
        }

        //Move in Circle around player between **PerformAttack**
        if (enemy.agent.remainingDistance <= 0.5f)
        {
            SetNewDestination(enemy);
        }
    }

    private Vector2 RandomPointOnCircle()
    {
        var angleOffset = (Random.Range(Mathf.PI * 0.25f, Mathf.PI * 0.5f) * Random.Range(1, 2) * 2) - 3f;
        return new Vector2(Mathf.Cos(angleOffset), Mathf.Sin(angleOffset));
    }

    private void SetNewDestination(EnemyStateManager enemy)
    {
        enemy.destination =  enemy.playerData.position * RandomPointOnCircle() * enemy.enemyStats.attackRange * 0.9f;
        enemy.agent.destination = enemy.destination;
    }
}
