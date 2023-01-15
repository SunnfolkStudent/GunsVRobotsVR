using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPerformAttackState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.agent.speed = 0f;
        
        enemy.animator.Play("AttackWindup");
        
        enemy.SwitchState(enemy.EngageState);
    }

    public override void HandleState(EnemyStateManager enemy)
    {
        if (enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            enemy.SwitchState(enemy.EngageState);
        }
    }
}
