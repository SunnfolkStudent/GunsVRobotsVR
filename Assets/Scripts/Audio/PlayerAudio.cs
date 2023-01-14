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
        AudioMixer.instance.TryAddVoiceSource(AudioMixer.Source.Player, this.gameObject);
        AudioMixer.instance.TryAddSfxSource(AudioMixer.Source.Player, this.gameObject);
        AudioMixer.instance.TryAddVoiceEvent(AudioMixer.Source.Player, "OnPlayerHit", OnPlayerHit);
        AudioMixer.instance.TryAddVoiceEvent(AudioMixer.Source.Player, "OnEnemyHit", OnEnemyHit);
        AudioMixer.instance.TryAddVoiceEvent(AudioMixer.Source.Player, "OnEnemyKill", OnEnemyKill);
    }

    private void Update()
    {
    }

    public void OnPlayerHit()
    {
        if (AudioMixer.instance.TryGetVoiceSource(AudioMixer.Source.Player, out AudioSource source))
        {
            source.PlayOneShot(playerHit);
            if (UnityEngine.Random.Range(0f, 1f) < 0.3f)
                source.PlayOneShot(playerAngry);
        }
    }
    private void OnEnemyHit()
    {
        if (UnityEngine.Random.Range(0f, 1f) < 0.3f)
            if (AudioMixer.instance.TryGetVoiceSource(AudioMixer.Source.Player, out AudioSource source))
                source.PlayOneShot(playerHitEnemy);
    }
    private void OnEnemyKill()
    {
        if (UnityEngine.Random.Range(0f, 1f) < 0.3f)
            if (AudioMixer.instance.TryGetVoiceSource(AudioMixer.Source.Player, out AudioSource source))
                source.PlayOneShot(playerKillEnemy);
    }
}
