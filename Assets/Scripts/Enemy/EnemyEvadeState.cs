using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvadeState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        
    }

    public override void HandleState(EnemyStateManager enemy)
    {
        //Unique for each enemy
        //Base: Dash around player towards Flank
        //Slerp
        //Once Evade is finished Goto -> **MoveTowards** or **Engage**
        if (enemy.distanceToPlayer > enemy.attackRange)
        {
            enemy.SwitchState(enemy.MoveTowardsState);
            return;
        }
        else
        {
            enemy.SwitchState(enemy.EngageState);
            return;
        }
    }
}
