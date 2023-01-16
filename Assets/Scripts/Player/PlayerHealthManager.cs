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

    [HideInInspector]
    public PlayerAudio _playerAudio;

    private void Start()
    {
        //Reset player position relative to the XR Rig
        gameObject.transform.parent.localPosition = Vector3.zero;
        gameObject.transform.parent.localRotation = Quaternion.identity;
        
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

        AudioManager.instance.PlaySound(AudioManager.SoundType.Voice, AudioManager.Source.Player, _playerAudio.onHit_Hurt);
        if (UnityEngine.Random.Range(0f, 1f) < 0.4f)
            AudioManager.instance.PlaySound(AudioManager.SoundType.Voice, AudioManager.Source.Player, _playerAudio.onHit_Voice);

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
            //StartCoroutine(ResetStage());
        }
    }

    private IEnumerator ResetStage()
    {
        //Fade out
        var screenFade = GetComponentInChildren<ScreenFade>();
        screenFade.FadeOut();
        yield return new WaitForSeconds(screenFade.fadeDuration);
        
        //Reset the stage
        PauseManager.IsPaused = true;
        
        //Remove enemies
        foreach (var activeEnemy in EnemyPoolController.CurrentEnemyPoolController.activeEnemies)
        {
            EnemyPoolController.CurrentEnemyPoolController.DestroyEnemy(activeEnemy);
        }
        
        //Reset the enemy-spawner
        EnemyPoolController.CurrentEnemyPoolController.GetComponent<EnemySpawnController>().StartSpawningFromStart();
        
        //Reset or remove all bullets
        foreach (var activePlayerBullet in BulletPoolController.CurrentBulletPoolController.activePlayerBullets)
        {
            BulletPoolController.CurrentBulletPoolController.RegisterPlayerBulletAsInactive(activePlayerBullet.GetComponent<PlayerBulletData>());
        }

        foreach (var activeEnemyBullet in BulletPoolController.CurrentBulletPoolController.activeEnemyBullets)
        {
            BulletPoolController.CurrentBulletPoolController.RegisterEnemyBulletAsInactive(activeEnemyBullet.GetComponent<EnemyBulletData>());
        }
        
        //Reset the gun swap system
        GetComponent<WeaponMain>().ResetWeaponState();
        
        //Reset player position relative to the XR Rig
        gameObject.transform.parent.localPosition = Vector3.zero;
        gameObject.transform.parent.localRotation = Quaternion.identity;

        PauseManager.IsPaused = false;
        
        //Fade in
        screenFade.FadeIn();
        yield return new WaitForSeconds(screenFade.fadeDuration);
    }
}
