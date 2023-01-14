using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomDialogueManager : MonoBehaviour
{
    public AudioSource enemyAudio;
    public AudioClip[] enemyAudioList;

    private int randomIndex;
    
    private void Start()
    {
        enemyAudio = GetComponentInChildren<AudioSource>();
    }

    private void Update()
    {
        randomIndex = Random.Range(0,enemyAudioList.Length);
                    enemyAudio.PlayOneShot(enemyAudioList[randomIndex]);
    }
}

