using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEngageState : EnemyBaseState
{
    public float attackDelay = 1f;
    public float attackTimer;

    public float evadeDelay = 2f;
    public float evadeTimer;

    public float evasionChance;

    private float _integrityOnPreviousFrame;
    private float _integrityOnCurrentFrame;
    
    public override void EnterState(EnemyStateManager enemy)
    {
        _integrityOnPreviousFrame = enemy.currentIntegrity;
        _integrityOnCurrentFrame = enemy.currentIntegrity;
    }

    public override void HandleState(EnemyStateManager enemy)
    {
        _integrityOnPreviousFrame = _integrityOnCurrentFrame;
        _integrityOnPreviousFrame = enemy.currentIntegrity;
        
        enemy.distanceToPlayer = CalculateDistanceToPlayer(enemy);
        
        //Checks if a raycast towards the player hits any environment object.
        var directionTowardsPlayer = (enemy.playerData.position - enemy.transform.position).normalized;
        var isSightLineBlocked = Physics.Raycast(enemy.transform.position, directionTowardsPlayer,
            enemy.distanceToPlayer, enemy.whatIsEnvironment);

        //Move in Circle around player between attacks and evasions
        if (enemy.agent.remainingDistance <= 0.5f)
        {
            SetNewDestination(enemy, directionTowardsPlayer);
        }
        
        if (enemy.distanceToPlayer > enemy.enemyStats.attackRange * 1.2f)
        {
            enemy.SwitchState(enemy.MoveTowardsState);
            return;
        }
        
        //Evade
        //TODO: Add evasion timer.
        if (_integrityOnPreviousFrame > _integrityOnCurrentFrame && Time.time > evadeTimer + evadeDelay)
        {
            if (Random.Range(0f, 1f) <= evasionChance)
            {
                evadeTimer = Time.time;
                enemy.SwitchState(enemy.EvadeState);
                return;
            }
        }

        //Attack
        if (!isSightLineBlocked && Time.time > attackTimer + attackDelay/*(enemy.enemyStats.gunData.fireRate / 60f)*/)
        {
            attackTimer = Time.time;
            enemy.SwitchState(enemy.PerformAttackState);
            return;
        }
    }

    private Vector3 RotateRandomAmount(Vector3 direction)
    {
        var angleOffset = Random.Range(Mathf.PI * 0.25f, Mathf.PI * 0.5f) * (Random.Range(0, 2) * 2f - 1f);
        var cos = Mathf.Cos(angleOffset);
        var sin = Mathf.Sin(angleOffset);
        return new Vector3(cos * direction.x - sin * direction.z, 0f, sin * direction.x + cos * direction.z);
    }

    private void SetNewDestination(EnemyStateManager enemy, Vector3 directionTowardsPlayer)
    {
        enemy.destination = enemy.playerData.position + enemy.enemyStats.attackRange * 0.9f * RotateRandomAmount(-directionTowardsPlayer).normalized;
        enemy.agent.destination = enemy.destination;
    }
}
