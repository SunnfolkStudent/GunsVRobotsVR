using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class BossInterruptChargeState : BossBaseState
    {
        public override void EnterState(BossStateManager boss)
        {
            boss.animator.Play("Boss_Idle");
        }

        public override void HandleState(BossStateManager boss)
        {
            boss.rb.velocity = new Vector3(0f, -boss.fallSpeed, 0f);

            if (boss.transform.position.y <= boss.defaultHeight)
            {
                boss.transform.position =
                    new Vector3(boss.transform.position.x, boss.defaultHeight, boss.transform.position.z);
                boss.SwitchState(boss.ShieldState);
            }
        }
    }
}
