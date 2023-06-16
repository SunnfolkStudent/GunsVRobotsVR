using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Boss
{
    public class BossShieldState : BossBaseState
    {
        private float _endTime;
        private float _spawnNextSentryProjectileAtTime;
        
        public override void EnterState(BossStateManager boss)
        {
            boss.animator.Play("Boss_Idle");
            _endTime = Time.time + boss.maxShieldingTime;
            _spawnNextSentryProjectileAtTime = Time.time + boss.timeBetweenSentries;
            
            boss.shield.SetActive(true);
            boss.StartCoroutine(EnemyPoolController.CurrentEnemyPoolController.GetComponent<EnemySpawnController>().SpawnWave());
        }

        public override void HandleState(BossStateManager boss)
        {
            boss.rb.velocity = Vector3.zero;
            
            var numberOfNonProjectileEnemies =
                EnemyPoolController.CurrentEnemyPoolController.activeEnemies.Count(enemy =>
                    !enemy.TryGetComponent<SentryProjectileBehaviour>(out var projectile));

            if (Time.time > _endTime || numberOfNonProjectileEnemies == 0)
            {
                boss.shield.SetActive(false);
                boss.SwitchState(boss.IdleState);
                return;
            }

            if (Time.time > _spawnNextSentryProjectileAtTime)
            {
                _spawnNextSentryProjectileAtTime += boss.timeBetweenSentries;
                boss.FireSentryProjectile();
            }
        }
    }
}
