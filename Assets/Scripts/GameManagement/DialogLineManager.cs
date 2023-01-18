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
        else if (!playerAudio.isPlaying)
        {
            isTalking = false;
        }
    }

    public void NextVoiceLine()
    {
        //TODO: get input
        if (!playerAudio.isPlaying /*&& got input*/) 
            currentMsg++;
    }
    
    public void IsFinishedTalking()
    {
        if (!playerAudio.isPlaying /*&& got input*/)
        {
          isTalking = false;  
        }
        else
        {
            isTalking = true;
        }
    }
}

