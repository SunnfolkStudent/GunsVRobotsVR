using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

namespace Boss
{
    public class BossStateManager : MonoBehaviour
    {
        public BossBaseState currentState;
        public BossChargeState ChargeState = new BossChargeState();
        public BossFallState FallState = new BossFallState();
        public BossFreezeState FreezeState = new BossFreezeState();
        public BossIdleState IdleState = new BossIdleState();
        public BossInterruptChargeState InterruptChargeState = new BossInterruptChargeState();
        public BossRiseState RiseState = new BossRiseState();
        public BossShieldState ShieldState = new BossShieldState();
        public BossShootState ShootState = new BossShootState();
        public BossSinkState SinkState = new BossSinkState();
        
        public GameObject sentryProjectilePrefab;
        public float timeBetweenSentries;
        public float chargeDelayAfterShooting;
        public float chargeSpeed;
        public float chargeHeight;
        public float chargeDamage;
        public float maxChargeDistance = 50f;
        public float riseSpeed = 2f;
        public float fallSpeed;
        public float sinkSpeed;
        public float maxDistanceToPlayer;
        public float minDistanceToPlayer;
        public float maxIdleRaycastDistance;
        public float movementSpeed;
        public float defaultHeight;
        public float minimumTimeInIdleState;
        public string state;
        public Transform projectileSpawnPoint;

        public float frozenTimeAfterCrashing;
        public float maxShieldingTime;
    
        public float maxShield;
        public float maxArmour;
        public float maxIntegrity;
        public float currentShield;
        public float currentArmour;
        public float currentIntegrity;

        public float[] integrityStaggerTriggers;

        public LayerMask whatIsEnvironment;

        public PlayerData playerData;

        public Animator animator;
        public Rigidbody rb;
        public GameObject shield;

        [Header("Player voice")]
        public AudioClip[] onPlayerKillEnemy;
        public AudioClip onPlayerHitEnemy;
        private float timeSincePlayerVoice = 0;

        [Header("Enemy sfx")]
        public AudioClip[] onEnemyDeath;
        public AudioClip[] onEnemyHit;
        public AudioClip onEnemyMove;

        [Header("Gun effects")]
        public VisualEffect shotVFX;
        public AudioClip[] shotSFX;

        private bool _hasCrossedDamageThreshold;

        private void Awake()
        {
            currentShield = maxShield;
            currentArmour = maxArmour;
            currentIntegrity = maxIntegrity;
            
            defaultHeight = transform.position.y;
            SwitchState(IdleState);
        }

        private void Update()
        {
            state = currentState.GetType().Name;
            if (PauseManager.IsPaused) return;

            currentState.HandleState(this);
        }

        public void SwitchState(BossBaseState newState)
        {
            currentState = newState;
            currentState.EnterState(this);
        }

        public bool HasCrossedDamageThreshold()
        {
            if (!_hasCrossedDamageThreshold) return false;
            
            _hasCrossedDamageThreshold = false;
            return true;
        }

        public void FireProjectile()
        {
            Instantiate(sentryProjectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        }
        
        public void Shoot()
        {
            var randomAimOffset = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
            var directionTowardsPlayer = (playerData.position - transform.position).normalized;
            var fireDirection = Quaternion.LookRotation((directionTowardsPlayer + randomAimOffset).normalized, Vector3.up);
            EnemyPoolController.CurrentEnemyPoolController.SpawnEnemy(sentryProjectilePrefab, projectileSpawnPoint.position,
                fireDirection);
        }
        
        public void TakeDamage(float dmg, float armourPierce, float armourShred, float shieldPierce, float shieldDisrupt, float stunTime, float knockBack)
        {
            if (currentState == ShieldState) return;
            
            var previousIntegrity = currentIntegrity;
        
            //int index = EnemyPoolController.CurrentEnemyPoolController.activeEnemies.IndexOf(gameObject);
            //AudioManager.instance.PlaySound(AudioManager.SoundType.Sfx, AudioManager.Source.Enemy, onEnemyHit, index);
            //if (UnityEngine.Random.Range(0f, 1f) < 0.4f)
            //    AudioManager.instance.PlaySound(AudioManager.SoundType.Voice, AudioManager.Source.Player, onPlayerHitEnemy, index);

            if (currentShield >= 0)
            {
                currentShield -= ((dmg + armourPierce + armourShred + armourPierce) / 2 + shieldDisrupt);

                if (currentArmour > 0)
                {
                    currentArmour -= shieldPierce;
                }
            
                else
                {
                    currentIntegrity -= shieldPierce;
                }
            }

            if (currentShield <= 0 && currentArmour >= 0)
            {
                currentArmour -= ((dmg + armourPierce + shieldPierce + shieldDisrupt) / 2 + armourShred);
                currentIntegrity -= armourPierce;
            }

            if (currentShield <= 0 && currentArmour <= 0)
            {
                currentIntegrity -= (dmg + armourPierce + shieldPierce + shieldDisrupt + armourShred + armourPierce) / 2;
            }

            foreach (var trigger in integrityStaggerTriggers)
            {
                if (previousIntegrity > trigger && currentIntegrity <= trigger)
                {
                    _hasCrossedDamageThreshold = true;
                }
            }
        }

        private void OnTriggerEnter(Collider col)
        {
            if (currentState != ChargeState) return;

            if (col.CompareTag("Player"))
            {
                var player = col.GetComponentInParent<PlayerHealthManager>();
                player.TakeDamage(chargeDamage, 0f, 0f, 0f,
                    0f);
            }
            else if (col.CompareTag("Environment/LargeObstacle"))
            {
                SwitchState(FallState);
            }
        }
    }
}
