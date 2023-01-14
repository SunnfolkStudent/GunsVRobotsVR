using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using TMPro;
using UnityEngine;

public class DialogLineManager : MonoBehaviour
{
    [Header("Audio and Text")] [Space(10)]
    public AudioSource audio;
    public TMP_Text text;

    [Header("Lists")] [Space(10)]
    public AudioClip[] audioList;
    public string[] textList;
    
    public bool isTalking = false;
    
    [HideInInspector]
    public int currentMsg = 0;
    
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        text = GetComponent<TMP_Text>();
    }

    public void IsTalking()
    {
        if (audio.isPlaying)
        {
            isTalking = true;
            text.gameObject.SetActive(true);
            
            currentMsg = textList.Length;
            text.text = textList[currentMsg];
            audio.PlayOneShot(audioList[currentMsg]);
        }
        else
        {
            isTalking = false;
        }
    }

    private void IsFinishedTalking()
    {
        if (!audio.isPlaying  /* && clicked to proceed and there's no more text*/)
        {
          isTalking = false;  
          text.gameObject.SetActive(false);
        }
        else 
        {
            
        }
    }
}

