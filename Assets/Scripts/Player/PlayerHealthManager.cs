using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PowerUpManager))]
public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField]
    private PlayerData _playerData;

    public float maxIntegrity;
    public float CurrentIntegrity { get; private set; }
    public float maxShield;
    private float _currentShield;
    public float maxArmour;
    private float _currentArmour;

    private PlayerAudio _playerAudio;

    private void Start()
    {
        CurrentIntegrity = maxIntegrity;
        _currentShield = maxShield;
        _currentArmour = maxArmour;
        _playerAudio = GetComponentInChildren<PlayerAudio>();
    }

    private void Update()
    {
        _playerData.position = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            other.GetComponent<Pickup>().MoveTowardsPlayer(this);
        }
    }

    public void HealDamage(float amount)
    {
        CurrentIntegrity += amount;
        if (CurrentIntegrity > maxIntegrity)
        {
            CurrentIntegrity = maxIntegrity;
        }
    }

    public void RefillAmmo(float amount)
    {
        GetComponent<PowerUpManager>().RefillAmmo(amount);
    }

    public void TakeDamage(float dmg, float armourPierce, float armourShred, float shieldPierce, float shieldDisrupt)
    {
        print("I got hit today");
        _playerAudio.OnPlayerHit();

        if (_currentShield >= 0)
        {
            _currentShield -= ((dmg + armourPierce + armourShred + armourPierce) / 2 + shieldDisrupt);

            if (_currentArmour > 0)
            {
                _currentArmour -= shieldPierce;
            }
            
            else
            {
                CurrentIntegrity -= shieldPierce;
            }
        }

        if (_currentShield <= 0 && _currentArmour >= 0)
        {
            _currentArmour -= ((dmg + armourPierce + shieldPierce + shieldDisrupt) / 2 + armourShred);
            CurrentIntegrity -= armourPierce;
        }

        if (_currentShield <= 0 && _currentArmour <= 0)
        {
            CurrentIntegrity -= (dmg + armourPierce + shieldPierce + shieldDisrupt + armourShred + armourPierce) / 2;
        }

        if (CurrentIntegrity <= 0f)
        {
            print("I, the player, hath perished. Woe is me. Make me die where this print stands.");
        }
    }
}
