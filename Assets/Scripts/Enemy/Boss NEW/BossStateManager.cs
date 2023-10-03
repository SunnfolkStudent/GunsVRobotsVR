using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

namespace Boss
{
    public class BossStateManager : MonoBehaviour
    {
        public BossBaseState currentState;
        public BossInactiveState InactiveState = new BossInactiveState();
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
        public float sentryProjectileInitialSpeed;
        public GunData gunData;
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

        public EnemyHealthBar healthBar;
        public LayerMask whatIsEnvironment;

        public PlayerData playerData;

        public Animator animator;
        public Rigidbody rb;
        public GameObject shield;
        public Collider damagePlayer;

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
        private Vector3 _startPosition;
        public bool trigger;

        private void Awake()
        {
            _startPosition = transform.position;
            currentShield = maxShield;
            currentArmour = maxArmour;
            currentIntegrity = maxIntegrity;
            
            defaultHeight = transform.position.y;
            SwitchState(InactiveState);
        }

        private void Start()
        {
            healthBar.SetMaxValues(maxShield, maxArmour, maxIntegrity);
            healthBar.UpdateHealthBar(currentShield, currentArmour, currentIntegrity);
        }

        private void Update()
        {
            state = currentState.GetType().Name;
            if (PauseManager.IsPaused) return;
            
            currentState.HandleState(this);

            if (trigger)
            {
                trigger = false;
                Shoot();
            }
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

        public void FireSentryProjectile()
        {
            var directionTowardsPlayer = playerData.position - transform.position;
            directionTowardsPlayer.y = 0f;
            directionTowardsPlayer = directionTowardsPlayer.normalized;
            
            var fireDirection = Quaternion.LookRotation(directionTowardsPlayer, Vector3.up);
            var projectile = EnemyPoolController.CurrentEnemyPoolController.SpawnEnemy(sentryProjectilePrefab, transform.position,
                Quaternion.identity);
            //Instantiate(sentryProjectilePrefab, projectileSpawnPoint.position, Quaternion.LookRotation(projectileSpawnPoint.forward));
            
            projectile.GetComponentInChildren<NavMeshAgent>().velocity = projectile.transform.forward * sentryProjectileInitialSpeed;
            //shotVFX.Play();
            //AudioManager.instance.PlaySound(gameObject, shotSFX[Random.Range(0, shotSFX.Length)]);
        }
        
        public void Shoot()
        {
            var randomAimOffset = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
            
            var directionTowardsPlayer = playerData.position - transform.position;
            directionTowardsPlayer = directionTowardsPlayer.normalized;
            
            var fireDirection = Quaternion.LookRotation((directionTowardsPlayer + randomAimOffset).normalized, Vector3.up);
            BulletPoolController.CurrentBulletPoolController.SpawnEnemyBullet(gunData, projectileSpawnPoint.position, fireDirection);
            shotVFX.Play();
            //AudioManager.instance.PlaySound(gameObject, shotSFX[Random.Range(0, shotSFX.Length)]);
        }
        
        public void TakeDamage(float dmg, float armourPierce, float armourShred, float shieldPierce, float shieldDisrupt, float stunTime, float knockBack)
        {
            if (currentState == ShieldState) return;
            if (currentState == InactiveState) return;
            
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
            
            healthBar.UpdateHealthBar(currentShield, currentArmour, currentIntegrity);
        }

        private void OnTriggerEnter(Collider col)
        {
            if (currentState != ChargeState) return;

            if (col.CompareTag("Environment/LargeObstacle"))
            {
                SwitchState(FallState);
            }
        }

        public void ResetState()
        {
            transform.position = _startPosition;
            
            SwitchState(IdleState);

            currentShield = maxShield;
            currentArmour = maxArmour;
            currentIntegrity = maxIntegrity;
            
            healthBar.UpdateHealthBar(currentShield, currentArmour, currentIntegrity);
        }
    }
}
