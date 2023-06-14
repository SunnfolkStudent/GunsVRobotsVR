using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Boss
{
    public class BossShootState : BossBaseState
    {
        private float _endTime;
        
        public override void EnterState(BossStateManager boss)
        {
            var animationClip = boss.animator.runtimeAnimatorController.animationClips.First(clip => clip.name == "Boss_Shooting");
            _endTime = Time.time + animationClip.length + boss.chargeDelayAfterShooting;
            boss.animator.Play("Boss_Shooting");
        }

        public override void HandleState(BossStateManager boss)
        {
            boss.rb.velocity = Vector3.zero;
            boss.transform.LookAt(
                new Vector3(boss.playerData.position.x, boss.transform.position.y, boss.playerData.position.z));

            if (boss.HasCrossedDamageThreshold())
            {
                boss.SwitchState(boss.ShieldState);
                return;
            }

            if (Time.time > _endTime) boss.SwitchState(boss.RiseState);
        }
    }
}
