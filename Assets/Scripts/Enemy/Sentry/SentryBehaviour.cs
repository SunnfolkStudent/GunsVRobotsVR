using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class SentryBehaviour : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileInitialSpeed;
    public float checkForPlayerInterval;
    public LayerMask whatIsEnvironment;
    public PlayerData playerData;
    public Transform spawnPoint;
    
    public float maxShield;
    private float _currentShield;
    public float maxArmour;
    private float _currentArmour;
    public float maxIntegrity;
    private float _currentIntegrity;

    public EnemyHealthBar healthBar;
    private Animator _animator;
    private float _checkTimer;
    public GameObject explosionPrefab;

    [Header("EnemyVFX")] 
    public VisualEffect spawnVFX; 

    [Header("Player voice")]
    public AudioClip[] onPlayerKillEnemy;
    public AudioClip onPlayerHitEnemy;

    [Header("Enemy sfx")]
    public AudioClip[] onEnemyDeath;
    public AudioClip[] onEnemyHit;
    public AudioClip onEnemyMove;

    [Header("Gun effects")]
    public VisualEffect shotVFX;
    public AudioClip[] shotSFX;

    private void Awake()
    {
        _currentShield = maxShield;
        _currentArmour = maxArmour;
        _currentIntegrity = maxIntegrity;

        _checkTimer = Time.time + Random.Range(0f, checkForPlayerInterval);
        _animator = GetComponent<Animator>();
    }
    
    private void Start()
    {
        healthBar.SetMaxValues(maxShield, maxArmour, maxIntegrity);
        healthBar.UpdateHealthBar(_currentShield, _currentArmour, _currentIntegrity);
        healthBar.gameObject.SetActive(false);
        AudioManager.instance.TryAddSource(AudioManager.SoundType.Sfx, AudioManager.Source.Enemy, gameObject);
    }

    private void OnEnable()
    {
        spawnVFX.Play();
    }

    private void Update()
    {
        if (Time.time >= _checkTimer + checkForPlayerInterval)
        {
            _checkTimer = Time.time;
            _animator.Play("AimUp");
        }
    }

    public void ShootProjectile()
    {
        //Checks if a raycast from the player hits any environment object.
        var distanceToPlayer = (playerData.position - transform.position).magnitude;
        var directionTowardsPlayer = (playerData.position - transform.position).normalized;
        var isSightLineBlocked = Physics.Raycast(playerData.position, -directionTowardsPlayer,
            distanceToPlayer, whatIsEnvironment);

        if (isSightLineBlocked)
        {
            return;
        }
        
        var projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        EnemyPoolController.CurrentEnemyPoolController.activeEnemies.Add(projectile);
        projectile.GetComponentInChildren<NavMeshAgent>().velocity = projectile.transform.forward * projectileInitialSpeed;
        shotVFX.Play();
        AudioManager.instance.PlaySound(gameObject, shotSFX[Random.Range(0, shotSFX.Length)]);
    }

    public void TakeDamage(float dmg, float armourPierce, float armourShred, float shieldPierce, float shieldDisrupt)
    {
        int rand = UnityEngine.Random.Range(0, onEnemyHit.Length);
        AudioManager.instance.PlaySound(gameObject, onEnemyHit[rand]);
        if (UnityEngine.Random.Range(0f, 1f) < 0.4f)
            AudioManager.instance.PlaySound(AudioManager.SoundType.Voice, AudioManager.Source.Player, onPlayerHitEnemy);

        if (_currentShield >= 0)
        {
            _currentShield -= ((dmg + armourPierce + armourShred + armourPierce) / 2 + shieldDisrupt);

            if (_currentArmour > 0)
            {
                _currentArmour -= shieldPierce;
            }
            
            else
            {
                _currentIntegrity -= shieldPierce;
            }
        }

        if (_currentShield <= 0 && _currentArmour >= 0)
        {
            _currentArmour -= ((dmg + armourPierce + shieldPierce + shieldDisrupt) / 2 + armourShred);
            _currentIntegrity -= armourPierce;
        }

        if (_currentShield <= 0 && _currentArmour <= 0)
        {
            _currentIntegrity -= (dmg + armourPierce + shieldPierce + shieldDisrupt + armourShred + armourPierce) / 2;
        }

        if (_currentIntegrity <= 0f)
        {
            int i = Random.Range(0, onEnemyDeath.Length);
            AudioManager.instance.PlaySound(gameObject, onEnemyDeath[i]);
            AudioManager.instance.TryRemoveSource(AudioManager.SoundType.Sfx, AudioManager.Source.Enemy, gameObject);
            EnemyPoolController.CurrentEnemyPoolController.GetComponent<EnemySpawnController>().activeSentries.Remove(gameObject);
            
            var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            
            explosion.transform.localScale = new Vector3(2f, 2f, 2f);
            Destroy(gameObject);
        }
        
        healthBar.gameObject.SetActive(true);
        healthBar.UpdateHealthBar(_currentShield, _currentArmour, _currentIntegrity);
    }
}
