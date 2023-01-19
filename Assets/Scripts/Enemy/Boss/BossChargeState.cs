using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossChargeState : BossBaseState
{
    private Vector3 _chargeDirection;
    private float _chargeTimer;
    private Vector3 _startPosition;

    public bool isCharging;
    public RaycastHit chargeHit;
    
    public override void EnterState(BossStateManager boss)
    {
        _startPosition = boss.visuals.transform.localPosition;
        _chargeDirection = boss.playerData.position - boss.transform.position;
        _chargeDirection.y = 0f;
        _chargeDirection = _chargeDirection.normalized;
        NavMesh.SamplePosition(chargeHit.point,
            out NavMeshHit hit, 20f, NavMesh.AllAreas);
        boss.destination = hit.position;
        boss.agent.destination = boss.destination;
        
        _chargeTimer = Time.time;

        isCharging = true;
    }

    public override void HandleState(BossStateManager boss)
    {
        //Build up to the charge
        if (Time.time < _chargeTimer + boss.chargeDelayAfterShooting)
        {
            var fractionOfChargeDelay = (Time.time - _chargeTimer) / boss.chargeDelayAfterShooting;

            boss.visuals.transform.localPosition = _startPosition +
                                                   new Vector3(0f, Mathf.Lerp(0f, boss.chargeHeight, fractionOfChargeDelay), 0f);
            return;
        }

        //Exit charge if the boss has crashed into anything
        if (!isCharging)
        {
            boss.SwitchState(boss.RecoverState);
        }
        
        boss.agent.speed = boss.chargeSpeed;
    }
}
