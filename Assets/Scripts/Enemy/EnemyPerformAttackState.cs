using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPerformAttackState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Shoot(enemy);
        
        enemy.SwitchState(enemy.EngageState);
    }

    public override void HandleState(EnemyStateManager enemy)
    {
        
    }
    
    public void Shoot(EnemyStateManager enemy)
    {
        var clone = GameObject.Instantiate(enemy.GetComponent<EnemyWeaponMain>().Bullets[0], enemy.transform.position,
            enemy.transform.rotation);
        clone.transform.LookAt(enemy.playerData.position);
        var bulletData = clone.GetComponent<EnemyBulletData>();
        bulletData.gunData = enemy.enemyStats.gunData;
        
        Object.Destroy(clone, enemy.enemyStats.gunData.range);
    }
}
