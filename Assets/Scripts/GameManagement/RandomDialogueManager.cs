using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomDialogueManager : MonoBehaviour
{
    public AudioSource Audio;
    public AudioClip[] AudioList;

    private int randomIndex;
    
    private void Start()
    {
        Audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        randomIndex = Random.Range(0,AudioList.Length);
                    Audio.PlayOneShot(AudioList[randomIndex]);
    }
}

