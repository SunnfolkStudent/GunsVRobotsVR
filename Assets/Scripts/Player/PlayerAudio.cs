using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip playerHit;
    public AudioClip playerAngry;
    public AudioClip playerHitEnemy;
    public AudioClip playerKillEnemy;

    private void Awake()
    {
        AudioMixer.instance.AddVoiceSource(AudioMixer.Source.Player, gameObject);
        AudioMixer.instance.AddSfxSource(AudioMixer.Source.Player, gameObject);
        EnemyPoolController.CurrentEnemyPoolController.OnEnemyHit.AddListener(OnEnemyHit);
        EnemyPoolController.CurrentEnemyPoolController.OnEnemyKill.AddListener(OnEnemyKill);
    }

    public void OnPlayerHit()
    {
        AudioMixer.instance.voiceLines[AudioMixer.Source.Player].PlayOneShot(playerHit);
        if (UnityEngine.Random.Range(0f, 1f) < 0.3f)
            AudioMixer.instance.voiceLines[AudioMixer.Source.Player].PlayOneShot(playerAngry);
    }
    private void OnEnemyHit()
    {
        if (UnityEngine.Random.Range(0f, 1f) < 0.3f)
            AudioMixer.instance.voiceLines[AudioMixer.Source.Player].PlayOneShot(playerHitEnemy);
    }
    private void OnEnemyKill()
    {
        if (UnityEngine.Random.Range(0f, 1f) < 0.3f)
            AudioMixer.instance.voiceLines[AudioMixer.Source.Player].PlayOneShot(playerKillEnemy);
    }
}
