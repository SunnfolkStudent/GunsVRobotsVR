using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using TMPro;
using UnityEngine;

public class DialogLineManager : MonoBehaviour
{
    [Header("Audio and Text")] [Space(10)]
    public AudioSource _audio;
    public TMP_Text text;
   
    [Header("Lists")] [Space(10)]
    public AudioClip[] audioList;
    public string[] textList;
    
    public bool isTalking;
    
    private void Start()
    {
        _audio = GetComponentInChildren<AudioSource>();
        text = GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
       
    }

    private void IsTalking()
    {
        //TODO: if audio is playing istalking = true
        //if (_audio.PlayOneShot())
        {
            isTalking = true;
        }
        
        
    }

    private void IsFinishedTalking()
    {
        if (!_audio.isPlaying  /* && clicked to proceed*/)
        {
          isTalking = false;  
        }
    }
}

