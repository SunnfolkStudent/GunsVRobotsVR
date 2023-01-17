using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStaggerState : BossBaseState
{
    private float _staggerTimer;
    
    public override void EnterState(BossStateManager boss)
    {
        _staggerTimer = Time.time;
        boss.agent.speed = 0f;
        boss.agent.enabled = false;
        boss.animator.Play("Idle");
        
        boss.shield.transform.position = boss.transform.position;
        boss.shield.SetActive(true);
        
        EnemyPoolController.CurrentEnemyPoolController.GetComponent<EnemySpawnController>().SpawnWave();
    }

    public override void HandleState(BossStateManager boss)
    {
        if (Time.time > _staggerTimer + boss.staggerTime || EnemyPoolController.CurrentEnemyPoolController.activeEnemies.Count == 0)
        {
            boss.shield.SetActive(false);
            boss.agent.enabled = true;
            boss.SwitchState(boss.MoveTowardsState);
        }
    }
}
