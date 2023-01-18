using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossMoveTowardsState : BossBaseState
{
    private float _attackTimer;
    
    public override void EnterState(BossStateManager boss)
    {
        boss.agent.speed = boss.enemyStats.moveSpeed;
        
        var directionTowardsPlayer = (boss.playerData.position - boss.transform.position).normalized;
        SetNewDestination(boss, directionTowardsPlayer);
    }

    public override void HandleState(BossStateManager boss)
    {
        var directionTowardsPlayer = (boss.playerData.position - boss.transform.position).normalized;
        
        boss.distanceToPlayer = CalculateDistanceToPlayer(boss);

        //If the player has moved enough, update the agent's movement target
        if (Vector3.Magnitude(boss.destination - boss.playerData.position) >= 1f)
        {
            SetNewDestination(boss, directionTowardsPlayer);
        }

        //Checks if a raycast from the player hits any environment object.
        var isSightLineBlocked = Physics.Raycast(boss.playerData.position, -directionTowardsPlayer,
            boss.distanceToPlayer, boss.whatIsEnvironment);

        if (!isSightLineBlocked && Time.time > _attackTimer + boss.enemyStats.attackDelay)
        {
            _attackTimer = Time.time;
            boss.SwitchState(boss.ShootState);
        }
    }
    
    private Vector3 RotateRandomAmount(Vector3 direction)
    {
        var angleOffset = Random.Range(Mathf.PI * 0.25f, Mathf.PI * 0.5f) * (Random.Range(0, 2) * 2f - 1f);
        var cos = Mathf.Cos(angleOffset);
        var sin = Mathf.Sin(angleOffset);
        return new Vector3(cos * direction.x - sin * direction.z, 0f, sin * direction.x + cos * direction.z);
    }

    private void SetNewDestination(BossStateManager boss, Vector3 directionTowardsPlayer)
    {
        boss.destination = boss.playerData.position + boss.enemyStats.attackRange * 0.9f * RotateRandomAmount(-directionTowardsPlayer).normalized;
        boss.agent.destination = boss.destination;
    }
}
