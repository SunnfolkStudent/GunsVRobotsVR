using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        boss.shield.layer = LayerMask.NameToLayer("Ignore Raycast");
        boss.shield.SetActive(false);
        GameObject.Find("GameManager").GetComponent<GameManager>().SpawnNextLevelTrigger();
    }

    public override void HandleState(BossStateManager boss)
    {
        //The shield should have a collider, the tag "environment" and a NavMesh obstacle-component
    }
}
