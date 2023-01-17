using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChargeState : BossBaseState
{
    private Vector3 _chargeDirection;
    private float _chargeTimer;
    private Vector3 _startPosition;

    public bool isCharging;
    
    public override void EnterState(BossStateManager boss)
    {
        _startPosition = boss.transform.position;
        _chargeDirection = boss.playerData.position - _startPosition;
        _chargeDirection.y = 0f;
        _chargeDirection = _chargeDirection.normalized;
        _chargeTimer = Time.time;

        isCharging = true;
        
        boss.agent.enabled = false;
    }

    public override void HandleState(BossStateManager boss)
    {
        //Build up to the charge
        if (Time.time < _chargeTimer + boss.chargeDelayAfterShooting)
        {
            var fractionOfChargeDelay = (Time.time - _chargeTimer) / boss.chargeDelayAfterShooting;

            boss.transform.position = _startPosition +
                                      new Vector3(0f, Mathf.Lerp(0f, boss.chargeHeight, fractionOfChargeDelay), 0f);
            return;
        }

        //Exit charge if the boss has crashed into anything
        if (!isCharging)
        {
            boss.SwitchState(boss.RecoverState);
        }
        
        //Charge
        boss.rb.velocity = _chargeDirection * boss.chargeSpeed;
    }
}
