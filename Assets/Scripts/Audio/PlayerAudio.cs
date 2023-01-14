using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip playerHit;
    public AudioClip playerAngry;
    public AudioClip playerHitEnemy;
    public AudioClip playerKillEnemy;

    private void Start()
    {
        AudioManager.instance.TryAddVoiceSource(AudioManager.Source.Player, this.gameObject);
        AudioManager.instance.TryAddSfxSource(AudioManager.Source.Player, this.gameObject);
        AudioManager.instance.TryAddVoiceEvent(AudioManager.Source.Player, "OnPlayerHit", OnPlayerHit);
        AudioManager.instance.TryAddVoiceEvent(AudioManager.Source.Player, "OnEnemyHit", OnEnemyHit);
        AudioManager.instance.TryAddVoiceEvent(AudioManager.Source.Player, "OnEnemyKill", OnEnemyKill);
    }

    private void Update()
    {
    }

    public void OnPlayerHit()
    {
        if (AudioManager.instance.TryGetVoiceSource(AudioManager.Source.Player, out AudioSource source))
        {
            source.PlayOneShot(playerHit);
            if (UnityEngine.Random.Range(0f, 1f) < 0.3f)
                source.PlayOneShot(playerAngry);
        }
    }
    private void OnEnemyHit()
    {
        if (UnityEngine.Random.Range(0f, 1f) < 0.3f)
            if (AudioManager.instance.TryGetVoiceSource(AudioManager.Source.Player, out AudioSource source))
                source.PlayOneShot(playerHitEnemy);
    }
    private void OnEnemyKill()
    {
        if (UnityEngine.Random.Range(0f, 1f) < 0.3f)
            if (AudioManager.instance.TryGetVoiceSource(AudioManager.Source.Player, out AudioSource source))
                source.PlayOneShot(playerKillEnemy);
    }
}
