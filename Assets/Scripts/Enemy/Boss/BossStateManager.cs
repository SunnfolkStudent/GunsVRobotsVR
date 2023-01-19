using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class BossStateManager : MonoBehaviour
{
    public BossBaseState currentState;
    public BossInitialiseState InitialiseState = new BossInitialiseState();
    public BossMoveTowardsState MoveTowardsState = new BossMoveTowardsState();
    public BossShootState ShootState = new BossShootState();
    public BossChargeState ChargeState = new BossChargeState();
    public BossRecoverState RecoverState = new BossRecoverState();
    public BossStaggerState StaggerState = new BossStaggerState();
    public BossDeathState DeathState = new BossDeathState();

    public EnemyStats enemyStats;
    public float chargeDelayAfterShooting;
    public float chargeSpeed;
    public float chargeHeight;
    public float chargeDamage;
    public GameObject visuals;

    public float staggerTime;
    public float recoveryTime;
    
    public float currentShield;
    public float currentArmour;
    public float currentIntegrity;

    public float[] integrityStaggerTriggers;

    public LayerMask whatIsEnvironment;
    public NavMeshAgent agent;
    public Vector3 destination;

    public PlayerData playerData;
    public float distanceToPlayer;

    public Animator animator;
    public Rigidbody rb;
    public GameObject shield;

    [Header("Player voice")]
    public AudioClip onPlayerKillEnemy;
    public AudioClip onPlayerHitEnemy;

    [Header("Enemy sfx")]
    public AudioClip onEnemyDeath;
    public AudioClip onEnemyHit;
    public AudioClip onEnemyMove;

    public string state;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        currentState = InitialiseState;
        currentState.EnterState(this);
    }

    private void Start()
    {
        AudioManager.instance.TryAddSource(AudioManager.SoundType.Sfx, AudioManager.Source.Enemy, gameObject);
    }

    private void Update()
    {
        state = currentState.GetType().Name;
        if (PauseManager.IsPaused) return;

        if (currentIntegrity <= 0)
        {
            SwitchState(DeathState);
            return;
        }

        if (IsMoving())
        {
            //int index = EnemyPoolController.CurrentEnemyPoolController.activeEnemies.IndexOf(gameObject);
            //AudioManager.instance.PlaySound(AudioManager.SoundType.Sfx, AudioManager.Source.Enemy, onEnemyMove, index);
        }
        
        transform.LookAt(new Vector3(playerData.position.x, transform.position.y, playerData.position.z));
        
        currentState.HandleState(this);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision registered with: "+collision.gameObject.name);
        
        if (collision.gameObject.CompareTag("Environment/LargeObstacle"))
        {
            Debug.Log("Stopping charge");
            ChargeState.isCharging = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealthManager>().TakeDamage(chargeDamage, 0f, 0f, 0f, 0f);
        }
    }

    public void SwitchState(BossBaseState newState)
    {
        if (currentState == DeathState)
        {
            return;
        }
        
        print(newState.GetType().Name);
        
        currentState = newState;
        currentState.EnterState(this);
    }

    public void Shoot()
    {
        //TODO: Bytt ut med Sentry-projectile-spawning
        var randomAimOffset = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        var directionTowardsPlayer = (playerData.position - transform.position).normalized;
        var fireDirection = Quaternion.LookRotation((directionTowardsPlayer + randomAimOffset).normalized, Vector3.up);
        BulletPoolController.CurrentBulletPoolController.SpawnEnemyBullet(enemyStats.gunData, transform.position, fireDirection);
    }
    
    public void TakeDamage(float dmg, float armourPierce, float armourShred, float shieldPierce, float shieldDisrupt, float stunTime, float knockBack)
    {
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
                SwitchState(StaggerState);
            }
        }
    }
    
    public bool IsMoving()
    {
        return agent.speed >= 0.1f && agent.acceleration >= 0.1f && agent.remainingDistance > 0.1f;
    }
}
