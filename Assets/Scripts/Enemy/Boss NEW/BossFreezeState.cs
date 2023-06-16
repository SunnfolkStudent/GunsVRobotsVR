using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class BossFreezeState : BossBaseState
    {
        private float _startTime;
        private float _endTime;
        private Quaternion initialRotation;
        
        public override void EnterState(BossStateManager boss)
        {
            _endTime = Time.time + boss.frozenTimeAfterCrashing;
            initialRotation = boss.transform.rotation;
            boss.animator.Play("Boss_Idle");
        }

        public override void HandleState(BossStateManager boss)
        {
            if (boss.HasCrossedDamageThreshold())
            {
                boss.SwitchState(boss.ShieldState);
                return;
            }
            
            //Rotates the boss.
            var directionTowardsPlayer = boss.playerData.position - boss.transform.position;
            directionTowardsPlayer.y = 0f;
            directionTowardsPlayer = directionTowardsPlayer.normalized;

            var fraction = (Time.time + boss.frozenTimeAfterCrashing - _endTime) / boss.frozenTimeAfterCrashing;
            
            boss.transform.rotation = Quaternion.Lerp(boss.transform.rotation,
                Quaternion.FromToRotation(boss.transform.forward, directionTowardsPlayer), fraction) * initialRotation;
            
            boss.rb.velocity = Vector3.zero;
            
            if (Time.time >= _endTime) boss.SwitchState(boss.IdleState);
        }
    }
}