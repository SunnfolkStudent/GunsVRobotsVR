using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPerformAttackState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        
    }

    public override void HandleState(EnemyStateManager enemy)
    {
        //Use GunType and Attack
        
        enemy.SwitchState(enemy.EngageState);
    }
}
