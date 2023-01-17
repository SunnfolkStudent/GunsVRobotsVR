using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BossMoveTowardsState : BossBaseState
{
    private float _attackTimer;
    
    public override void EnterState(BossStateManager boss)
    {
        boss.agent.speed = boss.enemyStats.moveSpeed;
        
        boss.destination = boss.playerData.position;
        boss.agent.destination = boss.destination;
    }

    public override void HandleState(BossStateManager boss)
    {
        boss.distanceToPlayer = CalculateDistanceToPlayer(boss);

        //If the player has moved enough, update the agent's movement target
        if (Vector3.Magnitude(boss.destination - boss.playerData.position) >= 1f)
        {
            boss.destination = boss.playerData.position;
            boss.agent.destination = boss.destination;
        }

        //Checks if a raycast from the player hits any environment object.
        var directionTowardsPlayer = (boss.playerData.position - boss.transform.position).normalized;
        var isSightLineBlocked = Physics.Raycast(boss.playerData.position, -directionTowardsPlayer,
            boss.distanceToPlayer, boss.whatIsEnvironment);

        if (!isSightLineBlocked && Time.time > _attackTimer + boss.enemyStats.attackDelay)
        {
            _attackTimer = Time.time;
            boss.SwitchState(boss.ShootState);
        }
    }
}
