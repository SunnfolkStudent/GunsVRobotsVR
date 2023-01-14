using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using TMPro;
using UnityEngine;

public class DialogLineManager : MonoBehaviour
{

    public AudioSource playerAudio;
    public AudioClip[] audioList;
    
    public bool isTalking = false;
    
    [HideInInspector]
    public int currentMsg = 0;
    
    private void Start()
    {
        playerAudio = GetComponent<AudioSource>();
    }

    public void IsTalking()
    {
        if (playerAudio.isPlaying)
        {
            isTalking = true;
            
            currentMsg = audioList.Length;
            playerAudio.PlayOneShot(audioList[currentMsg]);
        }
        else
        {
            isTalking = false;
        }
    }

    private void IsFinishedTalking()
    {
        if (!playerAudio.isPlaying  /* && clicked to proceed and there's no more text*/)
        {
          isTalking = false;  
        }
        else
        {
            isTalking = true;
        }
    }
}

