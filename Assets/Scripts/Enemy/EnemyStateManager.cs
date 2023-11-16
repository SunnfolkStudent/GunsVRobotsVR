using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.VFX;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

public class EnemyStateManager : MonoBehaviour
{
    public string currentStateOutput;
    public EnemyBaseState currentState;
    public EnemyInactiveState InactiveState = new EnemyInactiveState();
    public EnemyInitialiseState InitialiseState = new EnemyInitialiseState();
    public EnemyMoveTowardsState MoveTowardsState = new EnemyMoveTowardsState();
    public EnemyEngageState EngageState = new EnemyEngageState();
    public EnemyPerformAttackState PerformAttackState = new EnemyPerformAttackState();
    public EnemyEvadeState EvadeState = new EnemyEvadeState();
    public EnemyKnockBackState KnockBackState = new EnemyKnockBackState();
    public EnemyDeathState DeathState = new EnemyDeathState();

    public EnemyStats enemyStats;
    public float spawnDuration;
    
    public float currentShield;
    public float currentArmour;
    public float currentIntegrity;

    public EnemyHealthBar healthBar;
    public LayerMask whatIsEnvironment;
    public NavMeshAgent agent;
    public Vector3 destination;

    public PlayerData playerData;
    public float distanceToPlayer;

    public GameObject[] lootDrops;
    public GameObject[] healthDrops;

    public int itemNum;
    public Animator animator;

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
    
    [Header("EnemyVFX")] 
    public VisualEffect spawnVFX;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentState = InitialiseState;
        currentState.EnterState(this);
    }

    private void Start()
    {
        healthBar.SetMaxValues(enemyStats.maxShield, enemyStats.maxArmour, enemyStats.maxIntegrity);
        healthBar.UpdateHealthBar(currentShield, currentArmour, currentIntegrity);
        AudioManager.instance.TryAddSource(AudioManager.SoundType.Sfx, AudioManager.Source.Enemy, gameObject);
    }

    private void OnEnable()
    {
        spawnVFX.Play();
    }

    private void Update()
    {
        currentStateOutput = currentState.GetType().Name;
        if (PauseManager.IsPaused) return;

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
        AudioManager.instance.PlaySound(gameObject, onEnemyMove, false);
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
        var directionTowardsPlayer = (playerData.position + new Vector3(0f, 1.2f, 0f) - transform.position).normalized;
        var fireDirection = Quaternion.LookRotation((directionTowardsPlayer + randomAimOffset).normalized, Vector3.up);
        BulletPoolController.CurrentBulletPoolController.SpawnEnemyBullet(enemyStats.gunData, transform.position, fireDirection);
        shotVFX.Play();
        AudioManager.instance.PlaySound(gameObject, shotSFX[Random.Range(0, shotSFX.Length)]);
    }

    public void TakeDamage(float dmg, float armourPierce, float armourShred, float shieldPierce, float shieldDisrupt, float stunTime, float knockBack)
    {
        if (currentState == InactiveState) return;

        if (float.IsNaN(dmg)) dmg = 0f;
        if (float.IsNaN(armourPierce)) armourPierce = 0f;
        if (float.IsNaN(armourShred)) armourShred = 0f;
        if (float.IsNaN(shieldPierce)) shieldPierce = 0f;
        if (float.IsNaN(shieldDisrupt)) shieldDisrupt = 0f;
        if (float.IsNaN(stunTime)) stunTime = 0f;
        if (float.IsNaN(knockBack)) knockBack = 0f;
        
        int index = EnemyPoolController.CurrentEnemyPoolController.activeEnemies.IndexOf(gameObject);
        int rand = UnityEngine.Random.Range(0, onEnemyHit.Length);
        AudioManager.instance.PlaySound(gameObject, onEnemyHit[rand]);
        if (UnityEngine.Random.Range(0f, 1f) < 0.4f && Time.time > timeSincePlayerVoice + 5f)
        {
            timeSincePlayerVoice = Time.time;
            AudioManager.instance.PlaySound(AudioManager.SoundType.Voice, AudioManager.Source.Player, onPlayerHitEnemy);
        }

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
        
        healthBar.UpdateHealthBar(currentShield, currentArmour, currentIntegrity);

        if (currentIntegrity <= 0)
        {
            SwitchState(DeathState);
            return;
        }
        
        EngageState.wasHitThisFrame = true;
    }
    
    // Sometimes returns true when enemy is gone for some reason
    public bool IsMoving()
    {
        return agent.speed >= 0.1f && agent.acceleration >= 0.1f && agent.remainingDistance > 0.1f;
    }
}
