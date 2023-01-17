using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        boss.shield.SetActive(false);
    }

    public override void HandleState(BossStateManager boss)
    {
        //The shield should have a collider, the tag "environment" and a NavMesh obstacle-component
    }
}
