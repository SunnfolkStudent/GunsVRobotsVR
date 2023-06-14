using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class BossRiseState : BossBaseState
    {
        public override void EnterState(BossStateManager boss)
        {
            boss.animator.Play("Boss_Idle");
        }

        public override void HandleState(BossStateManager boss)
        {
            boss.transform.LookAt(
                new Vector3(boss.playerData.position.x, boss.transform.position.y, boss.playerData.position.z));

            if (boss.HasCrossedDamageThreshold())
            {
                boss.SwitchState(boss.InterruptChargeState);
                return;
            }
            
            boss.rb.velocity = new Vector3(0f, boss.riseSpeed, 0f);

            if (boss.transform.position.y >= boss.chargeHeight)
            {
                boss.transform.position = new Vector3(boss.transform.position.x, boss.chargeHeight, boss.transform.position.z);
                
                var direction = boss.playerData.position - boss.transform.position;
                direction.y = 0f;
                boss.SwitchState(boss.ChargeState);
            }
        }
    }
}
