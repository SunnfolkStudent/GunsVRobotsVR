using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRecoverState : BossBaseState
{
    private float _recoveryTimer;
    private Vector3 _startPosition;
    
    public override void EnterState(BossStateManager boss)
    {
        _startPosition = boss.transform.position;
        _recoveryTimer = Time.time;
        boss.agent.speed = 0f;
    }

    public override void HandleState(BossStateManager boss)
    {
        var fractionOfRecoveryTime = (Time.time - _recoveryTimer) / boss.recoveryTime;

        boss.transform.position = _startPosition +
                                  new Vector3(0f, Mathf.Lerp(boss.chargeHeight, 0f, fractionOfRecoveryTime), 0f);
        
        if (Time.time > _recoveryTimer + boss.recoveryTime)
        {
            boss.SwitchState(boss.MoveTowardsState);
        }
    }
}
