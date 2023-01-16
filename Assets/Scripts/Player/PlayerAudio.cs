using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip onHit_Voice;
    public AudioClip onHit_Hurt;
    public AudioClip onPickupPowerUp;

    private void Start()
    {
        AudioManager.instance.TryAddSource(AudioManager.SoundType.Sfx, AudioManager.Source.Player, this.gameObject);
        AudioManager.instance.TryAddSource(AudioManager.SoundType.Voice, AudioManager.Source.Player, this.gameObject);
    }

    private void Update()
    {
        // Purely for testing. Can safely be removed
        /*if (UnityEngine.Random.Range(0f, 1f) < 0.3f)
            AudioManager.instance.PlaySound(AudioManager.SoundType.Sfx, AudioManager.Source.Player, playerHit);
        if (UnityEngine.Random.Range(0f, 1f) < 0.3f)
            AudioManager.instance.PlaySound(AudioManager.SoundType.Voice, AudioManager.Source.Player, playerAngry);*/
    }
}
