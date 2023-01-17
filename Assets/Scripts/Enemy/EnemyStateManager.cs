using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

[RequireComponent(typeof(GunSFXnVFXManager))]
public class EnemyStateManager : MonoBehaviour
{
    public EnemyBaseState currentState;
    public EnemyInitialiseState InitialiseState = new EnemyInitialiseState();
    public EnemyMoveTowardsState MoveTowardsState = new EnemyMoveTowardsState();
    public EnemyEngageState EngageState = new EnemyEngageState();
    public EnemyPerformAttackState PerformAttackState = new EnemyPerformAttackState();
    public EnemyEvadeState EvadeState = new EnemyEvadeState();
    public EnemyKnockBackState KnockBackState = new EnemyKnockBackState();
    public EnemyDeathState DeathState = new EnemyDeathState();

    public EnemyStats enemyStats;
    
    public float currentShield;
    public float currentArmour;
    public float currentIntegrity;

    public LayerMask whatIsEnvironment;
    public NavMeshAgent agent;
    public Vector3 destination;

    public PlayerData playerData;
    public float distanceToPlayer;

    public GameObject[] lootDrops;
    public GameObject[] healthDrops;

    public int itemNum;
    public Animator animator;

    private GunSFXnVFXManager gunSFXnVFXManager;

    [Header("Player voice")]
    public AudioClip onPlayerKillEnemy;
    public AudioClip onPlayerHitEnemy;

    [Header("Enemy sfx")]
    public AudioClip[] onEnemyDeath;
    public AudioClip onEnemyHit;
    public AudioClip onEnemyMove;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentState = InitialiseState;
        currentState.EnterState(this);
        gunSFXnVFXManager = GetComponent<GunSFXnVFXManager>();
    }

    private void Start()
    {
        AudioManager.instance.TryAddSource(AudioManager.SoundType.Sfx, AudioManager.Source.Enemy, gameObject);
    }

    private void Update()
    {
        if (PauseManager.IsPaused) return;

        if (currentIntegrity <= 0)
        {
            SwitchState(DeathState);
            return;
        }

        if (IsMoving())
        {
            StartCoroutine(playMovingSound());
        }

        transform.LookAt(new Vector3(playerData.position.x, transform.position.y, playerData.position.z));
        
        currentState.HandleState(this);
        
        EngageState.wasHitThisFrame = false;
    }

    IEnumerator playMovingSound()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 1f));
        int index = EnemyPoolController.CurrentEnemyPoolController.activeEnemies.IndexOf(gameObject);
        AudioManager.instance.TryPlaySound(AudioManager.SoundType.Sfx, AudioManager.Source.Enemy, onEnemyMove, index);
    }

    public void SwitchState(EnemyBaseState newState)
    {
        if (currentState == DeathState)
        {
            return;
        }
        currentState = newState;
        currentState.EnterState(this);
    }

    public void Shoot()
    {
        var randomAimOffset = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        var directionTowardsPlayer = (playerData.position - transform.position).normalized;
        var fireDirection = Quaternion.LookRotation((directionTowardsPlayer + randomAimOffset).normalized, Vector3.up);
        BulletPoolController.CurrentBulletPoolController.SpawnEnemyBullet(enemyStats.gunData, transform.position, fireDirection);
        gunSFXnVFXManager.onShoot();
    }
    
    public void TakeDamage(float dmg, float armourPierce, float armourShred, float shieldPierce, float shieldDisrupt, float stunTime, float knockBack)
    {
        int index = EnemyPoolController.CurrentEnemyPoolController.activeEnemies.IndexOf(gameObject);
        AudioManager.instance.PlaySound(AudioManager.SoundType.Sfx, AudioManager.Source.Enemy, onEnemyHit, index);
        if (UnityEngine.Random.Range(0f, 1f) < 0.4f)
            AudioManager.instance.PlaySound(AudioManager.SoundType.Voice, AudioManager.Source.Player, onPlayerHitEnemy, index);

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

        if (stunTime > 0f && knockBack > 0f && enemyStats.knockBackResistance < 1f)
        {
            KnockBackState.stateToReturnTo = currentState;
            KnockBackState.stunTime = stunTime;
            KnockBackState.knockBackForce = knockBack * enemyStats.knockBackResistance;
            KnockBackState.knockBackDirection = (playerData.position - transform.position).normalized;
            SwitchState(KnockBackState);
        }

        EngageState.wasHitThisFrame = true;
    }
    
    public bool IsMoving()
    {
        return agent.speed >= 0.1f && agent.acceleration >= 0.1f && agent.remainingDistance > 0.1f;
    }
}
