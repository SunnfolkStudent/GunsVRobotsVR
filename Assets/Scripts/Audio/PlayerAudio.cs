using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip playerHit;
    public AudioClip playerAngry;
    // Needs to be move to where they are called
    public AudioClip playerHitEnemy;
    public AudioClip playerKillEnemy;

    private void Start()
    {
        AudioManager.instance.TryAddSource(AudioManager.SoundType.Sfx, AudioManager.Source.Player, this.gameObject);
        AudioManager.instance.TryAddSource(AudioManager.SoundType.Voice, AudioManager.Source.Player, this.gameObject);
    }

    private void Update()
    {
        // Purely for testing. Can safely be removed
        /*AudioManager.instance.PlaySound(AudioManager.SoundType.Sfx, AudioManager.Source.Player, playerHit);
        AudioManager.instance.PlaySound(AudioManager.SoundType.Voice, AudioManager.Source.Player, playerAngry);*/
    }
}
