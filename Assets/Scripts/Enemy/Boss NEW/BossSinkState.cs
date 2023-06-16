using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class BossSinkState : BossBaseState
    {
        private Vector3 initialForward;
        
        public override void EnterState(BossStateManager boss)
        {
            initialForward = boss.transform.forward;
            boss.animator.Play("Boss_Idle");
        }

        public override void HandleState(BossStateManager boss)
        {
            //Rotates the boss while sinking.
            var directionTowardsPlayer = boss.playerData.position - boss.transform.position;
            directionTowardsPlayer.y = 0f;
            directionTowardsPlayer = directionTowardsPlayer.normalized;

            var fraction = 1f - (boss.transform.position.y - boss.defaultHeight) / (boss.chargeHeight - boss.defaultHeight);
            
            boss.transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(initialForward, Vector3.up), 
                Quaternion.LookRotation(directionTowardsPlayer, Vector3.up), fraction);

            //Handles speed, position and state changes.
            boss.rb.velocity = new Vector3(0f, -boss.sinkSpeed, 0f);
            
            if (boss.transform.position.y <= boss.defaultHeight)
            {
                boss.transform.position =
                    new Vector3(boss.transform.position.x, boss.defaultHeight, boss.transform.position.z);
                if (boss.HasCrossedDamageThreshold())
                {
                    boss.SwitchState(boss.ShieldState);
                }
                else
                {
                    boss.SwitchState(boss.IdleState);
                }

                return;
            }
            
            if (boss.HasCrossedDamageThreshold())
            {
                boss.SwitchState(boss.InterruptChargeState);
            }
        }
    }
}