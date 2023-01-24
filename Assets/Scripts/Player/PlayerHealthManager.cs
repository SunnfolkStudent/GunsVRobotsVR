using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        AudioManager.instance.fmodManager.SetHealth(CurrentIntegrity);
    }

    private void Update()
    {
        _playerData.position = transform.parent.position;
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
        AudioManager.instance.fmodManager.SetHealth(CurrentIntegrity);

        if (CurrentIntegrity <= 0f)
        { 
            StartCoroutine(ResetStage());
        }
    }

    private IEnumerator ResetStage()
    {
        // Stop playing
        AudioManager.instance.fmodManager.StopPlaying();
        //Fade out
        var screenFade = GetComponentInChildren<ScreenFade>();
        screenFade.FadeOut();
        yield return new WaitForSeconds(screenFade.fadeDuration);
        
        //Reset the stage
        PauseManager.IsPaused = true;
        Time.timeScale = 0f;

        AudioManager.instance.TryClear(AudioManager.SoundType.Sfx, AudioManager.Source.Enemy);

        //Remove enemies
        var enemies = EnemyPoolController.CurrentEnemyPoolController.activeEnemies.ToList();
        foreach (var activeEnemy in enemies)
        {
            EnemyPoolController.CurrentEnemyPoolController.DestroyEnemy(activeEnemy);
        }
        
        //Remove all health and ammo pickups
        foreach (var pickup in GameObject.FindGameObjectsWithTag("Pickup"))
        {
            Destroy(pickup);
        }

        //Reset the enemy-spawner
        EnemyPoolController.CurrentEnemyPoolController.GetComponent<EnemySpawnController>().StartSpawningFromStart();
        
        //Reset or remove all bullets
        var playerBullets = BulletPoolController.CurrentBulletPoolController.activePlayerBullets.ToList();
        foreach (var activePlayerBullet in playerBullets)
        {
            BulletPoolController.CurrentBulletPoolController.DestroyPlayerBullet(activePlayerBullet.GetComponent<PlayerBulletData>());
        }
        
        var enemyBullets = BulletPoolController.CurrentBulletPoolController.activeEnemyBullets.ToList();
        foreach (var activeEnemyBullet in enemyBullets)
        {
            BulletPoolController.CurrentBulletPoolController.RegisterEnemyBulletAsInactive(activeEnemyBullet.GetComponent<EnemyBulletData>());
        }
        
        //Reset the gun swap system
        GetComponentInChildren<WeaponMain>().ResetWeaponState();
        
        //Reset player position relative to the XR Rig
        gameObject.transform.parent.localPosition = Vector3.zero;
        gameObject.transform.parent.localRotation = Quaternion.identity;
        
        //Reset player health
        CurrentIntegrity = maxIntegrity;
        _currentShield = maxShield;
        _currentArmour = maxArmour;

        PauseManager.IsPaused = false;
        Time.timeScale = 1f;


        //Fade in
        screenFade.FadeIn();
        yield return new WaitForSeconds(screenFade.fadeDuration);

        // Start playing fmod again
        AudioManager.instance.fmodManager.SetHealth(100);
        AudioManager.instance.fmodManager.StartPlaying();
    }
}
