using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class BossStateManager : MonoBehaviour
{
    public BossBaseState currentState;
    public BossInitialiseState InitialiseState = new BossInitialiseState();
    public BossMoveTowardsState MoveTowardsState = new BossMoveTowardsState();
    public BossShootState ShootState = new BossShootState();
    public BossRecoverState RecoverState = new BossRecoverState();
    public BossStaggerState StaggerState = new BossStaggerState();
    public BossDeathState DeathState = new BossDeathState();

    public EnemyStats enemyStats;
    public GameObject sentryProjectilePrefab;
    public float chargeDelayAfterShooting;
    public float chargeSpeed;
    public float chargeHeight;
    public float chargeDamage;
    public GameObject visuals;
    public Transform projectileSpawnPoint;

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
    //public Rigidbody rb;
    public GameObject shield;

    [Header("Player voice")]
    public AudioClip[] onPlayerKillEnemy;
    public AudioClip onPlayerHitEnemy;
    //private float timeSincePlayerVoice = 0;

    [Header("Enemy sfx")]
    public AudioClip[] onEnemyDeath;
    public AudioClip[] onEnemyHit;
    public AudioClip onEnemyMove;

    [Header("Gun effects")]
    public VisualEffect shotVFX;
    public AudioClip[] shotSFX;

    public string state;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        //rb = GetComponent<Rigidbody>();
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
            StartCoroutine(playMovingSound());
        }
        
        transform.LookAt(new Vector3(playerData.position.x, transform.position.y, playerData.position.z));
        
        currentState.HandleState(this);
    }
    
    IEnumerator playMovingSound()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 1f));
        AudioManager.instance.PlaySound(gameObject, onEnemyMove, false);
    }

    /*public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision registered with: "+collision.gameObject.name);
        
        if (collision.gameObject.CompareTag("Environment/LargeObstacle"))
        {
            Debug.Log("Stopping charge");
            ChargeState.isCharging = false;
        }
    }*/

    /*public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<PlayerHealthManager>().TakeDamage(chargeDamage, 0f, 0f, 0f, 0f);
        }
    }*/

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
        var randomAimOffset = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        var directionTowardsPlayer = (playerData.position - transform.position).normalized;
        var fireDirection = Quaternion.LookRotation((directionTowardsPlayer + randomAimOffset).normalized, Vector3.up);
        EnemyPoolController.CurrentEnemyPoolController.SpawnEnemy(sentryProjectilePrefab, projectileSpawnPoint.position,
            fireDirection);
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
