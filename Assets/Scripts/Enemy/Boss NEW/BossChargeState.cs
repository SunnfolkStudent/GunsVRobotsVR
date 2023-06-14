using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class BossChargeState : BossBaseState
    {
        private Vector3 _startPoint;
        
        public override void EnterState(BossStateManager boss)
        {
            _startPoint = boss.transform.position;
            boss.animator.Play("Charge_Buildup");
        }

        public override void HandleState(BossStateManager boss)
        {
            boss.rb.velocity = boss.transform.forward * boss.chargeSpeed;

            if (boss.HasCrossedDamageThreshold())
            {
                boss.SwitchState(boss.InterruptChargeState);
                return;
            }

            if (Vector3.Distance(boss.transform.position, _startPoint) >= boss.maxChargeDistance)
            {
                boss.SwitchState(boss.SinkState);
            }
        }
    }
}