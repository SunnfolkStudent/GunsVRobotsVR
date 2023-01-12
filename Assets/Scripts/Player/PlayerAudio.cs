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

    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        EnemyPoolController.CurrentEnemyPoolController.OnEnemyHit.AddListener(OnEnemyHit);
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
            source.PlayOneShot(playerHit);
    }
    public void OnEnemyKill()
    {
        if (UnityEngine.Random.Range(0f, 1f) < 0.3f)
            source.PlayOneShot(playerHit);
    }
}
