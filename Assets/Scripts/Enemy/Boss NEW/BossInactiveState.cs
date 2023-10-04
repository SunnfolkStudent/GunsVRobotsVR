using UnityEngine;

namespace Boss
{
    public class BossInactiveState : BossBaseState
    {
        private float _startTime;

        public override void EnterState(BossStateManager boss)
        {
            _startTime = Time.time;
        }

        public override void HandleState(BossStateManager boss)
        {
            boss.transform.LookAt(
                new Vector3(boss.playerData.position.x, boss.transform.position.y, boss.playerData.position.z));
            
            if (Time.time > _startTime + 13f)
            {
                boss.SwitchState(boss.IdleState);
            }
        }
    }
}