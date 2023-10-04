using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class BossDeathState : BossBaseState
    {
        private float _startTime;
        
        public override void EnterState(BossStateManager boss)
        {
            _startTime = Time.time;
            boss.Die();
        }

        public override void HandleState(BossStateManager boss)
        {
            if (Time.time > _startTime + 18f)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().OnNextLevelInteract();
            }
        }
    }
}
