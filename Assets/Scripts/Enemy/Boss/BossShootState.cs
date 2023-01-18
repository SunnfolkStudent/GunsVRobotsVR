using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShootState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        //Start shoot animation
        boss.agent.ResetPath();
        boss.agent.speed = 0f;
        boss.animator.Play("AimUp");
    }

    public override void HandleState(BossStateManager boss)
    {
        if (boss.animator.GetCurrentAnimatorStateInfo(0).IsName("AimUp"))
        {
            return;
        }
        
        //Checks if a boxcast can reach the player without hitting environment in front of the player.
        var directionTowardsPlayer = (boss.playerData.position - boss.transform.position).normalized;
        var canCharge = !Physics.BoxCast(boss.transform.position, boss.GetComponent<CapsuleCollider>().bounds.extents, directionTowardsPlayer,
            boss.transform.rotation, boss.distanceToPlayer, boss.whatIsEnvironment);

        if (canCharge)
        {
            boss.SwitchState(boss.ChargeState);
        }
        else
        {
            boss.SwitchState(boss.MoveTowardsState);
        }
    }
}
