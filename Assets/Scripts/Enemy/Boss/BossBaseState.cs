using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossBaseState
{
    public abstract void EnterState(BossStateManager boss);
    public abstract void HandleState(BossStateManager boss);
    
    public float CalculateDistanceToPlayer(BossStateManager boss)
    {
        return Vector3.Distance(boss.transform.position, boss.playerData.position);
    }
}