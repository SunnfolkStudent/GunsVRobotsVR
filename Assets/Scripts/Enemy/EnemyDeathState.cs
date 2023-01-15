using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDeathState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        // Player sometimes makes sounds when the enemy is killed
        if (AudioManager.instance.TryGetVoiceEvent(AudioManager.Source.Player, "OnEnemyKill", out UnityEngine.Events.UnityEvent e))
            e.Invoke();
        
        EnemyStateManager.Instantiate(enemy.lootDrops[enemy.itemNum], enemy.transform.position, Quaternion.identity);
        EnemyStateManager.Instantiate(enemy.healthDrops[enemy.itemNum], enemy.transform.position, Quaternion.identity);
        
        EnemyPoolController.CurrentEnemyPoolController.RegisterEnemyAsInactive(enemy);
    }

    public override void HandleState(EnemyStateManager enemy)
    {
        
    }
    
}
