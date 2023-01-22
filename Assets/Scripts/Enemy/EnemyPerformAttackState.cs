using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPerformAttackState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.agent.isStopped = true;
        
        enemy.animator.Play("AimUp");
    }

    public override void HandleState(EnemyStateManager enemy)
    {
        if (enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            enemy.agent.isStopped = false;
            enemy.SwitchState(enemy.EngageState);
        }
    }
}
