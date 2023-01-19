using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossRecoverState : BossBaseState
{
    private float _recoveryTimer;
    private Vector3 _startPosition;
    
    public override void EnterState(BossStateManager boss)
    {
        _startPosition = boss.visuals.transform.localPosition;
        _recoveryTimer = Time.time;
        boss.agent.speed = 0f;
    }

    public override void HandleState(BossStateManager boss)
    {
        var fractionOfRecoveryTime = (Time.time - _recoveryTimer) / (boss.recoveryTime * 2f);

        boss.visuals.transform.localPosition = _startPosition +
                                               new Vector3(0f, Mathf.Lerp(boss.chargeHeight, 0f, fractionOfRecoveryTime), 0f);
        
        if (Time.time > _recoveryTimer + boss.recoveryTime)
        {
            /*NavMesh.SamplePosition(boss.visuals.transform.position, out var hit, 10f, NavMesh.AllAreas);
            if (hit.hit)
            {*/
                //boss.transform.position = hit.position;
                boss.SwitchState(boss.MoveTowardsState);
            /*}
            else
            {
                Debug.Log("Could not find a point on the NavMesh.");
            }*/
        }
    }
}
