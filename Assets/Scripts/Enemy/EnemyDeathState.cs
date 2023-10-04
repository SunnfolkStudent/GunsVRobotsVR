using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDeathState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        int randDeathSound = UnityEngine.Random.Range(0, enemy.onEnemyDeath.Length);
        int randPlayerSound = UnityEngine.Random.Range(0, enemy.onPlayerKillEnemy.Length);
        AudioManager.instance.PlaySound(enemy.gameObject, enemy.onEnemyDeath[randDeathSound]);
        if (UnityEngine.Random.Range(0f, 1f) < 0.4f)
        {
            // If player voice becomes annoying add functionality to pause the source
            AudioManager.instance.PlaySound(AudioManager.SoundType.Voice, AudioManager.Source.Player, enemy.onPlayerKillEnemy[randPlayerSound]);
        }

        if (UnityEngine.Random.Range(0f, 1f) < 0.4f)
        {
            EnemyStateManager.Instantiate(enemy.lootDrops[enemy.itemNum], enemy.transform.position, Quaternion.identity);
        }
        
        if (UnityEngine.Random.Range(0f, 1f) < 0.4f)
        {
            EnemyStateManager.Instantiate(enemy.healthDrops[enemy.itemNum], enemy.transform.position, Quaternion.identity);
        }

        // Removing the audio source to prevent memory leak
        AudioManager.instance.TryRemoveSource(AudioManager.SoundType.Sfx, AudioManager.Source.Enemy, enemy.gameObject);
        EnemyPoolController.CurrentEnemyPoolController.DestroyEnemy(enemy.gameObject);
    }

    public override void HandleState(EnemyStateManager enemy)
    {
        
    }
    
}
