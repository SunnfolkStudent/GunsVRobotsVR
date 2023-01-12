using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip playerHit;
    public AudioClip playerAngry;
    public AudioClip playerHitEnemy;
    public AudioClip playerKillEnemy;

    private AudioSource source;

    private void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        EnemyPoolController.CurrentEnemyPoolController.OnEnemyHit.AddListener(OnEnemyHit);
        EnemyPoolController.CurrentEnemyPoolController.OnEnemyKill.AddListener(OnEnemyKill);
    }

    public void OnPlayerHit()
    {
        source.PlayOneShot(playerHit);
        if (UnityEngine.Random.Range(0f, 1f) < 0.3f)
            source.PlayOneShot(playerAngry);
    }
    public void OnEnemyHit()
    {
        if (UnityEngine.Random.Range(0f, 1f) < 0.3f)
            source.PlayOneShot(playerHitEnemy);
    }
    public void OnEnemyKill()
    {
        if (UnityEngine.Random.Range(0f, 1f) < 0.3f)
            source.PlayOneShot(playerKillEnemy);
    }
}
