using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SentryProjectileBehaviour : MonoBehaviour
{
    public PlayerData playerData;
    public float damage = 20f;
    
    public float maxShield;
    private float _currentShield;
    public float maxArmour;
    private float _currentArmour;
    public float maxIntegrity;
    private float _currentIntegrity;
    
    private NavMeshAgent _agent;
    
    [Header("Player voice")]
    public AudioClip onPlayerKillEnemy;
    public AudioClip onPlayerHitEnemy;

    [Header("Enemy sfx")]
    public AudioClip[] onEnemyDeath;
    public AudioClip onEnemyHit;
    public AudioClip onEnemyMove;
    
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.destination = playerData.position;
        AudioManager.instance.TryAddSource(AudioManager.SoundType.Sfx, AudioManager.Source.Enemy, gameObject);
    }

    private void Update()
    {
        //If the player has moved enough, update the agent's movement target
        if (Vector3.Magnitude(_agent.destination - playerData.position) >= 1f)
        {
            _agent.destination = playerData.position;
        }
        AudioManager.instance.PlaySound(gameObject, onEnemyMove, false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealthManager>().TakeDamage(damage, 0f, 0f, 0f, 0f);
            AudioManager.instance.TryRemoveSource(AudioManager.SoundType.Sfx, AudioManager.Source.Enemy, gameObject);
            Destroy(gameObject);
        }
    }
    
    public void TakeDamage(float dmg, float armourPierce, float armourShred, float shieldPierce, float shieldDisrupt)
    {
        int index = EnemyPoolController.CurrentEnemyPoolController.activeEnemies.IndexOf(gameObject);
        AudioManager.instance.PlaySound(gameObject, onEnemyHit);
        if (UnityEngine.Random.Range(0f, 1f) < 0.1f)
            AudioManager.instance.PlaySound(AudioManager.SoundType.Voice, AudioManager.Source.Player, onPlayerHitEnemy, index);

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
            EnemyPoolController.CurrentEnemyPoolController.DestroyEnemy(gameObject);
        }
    }
}
