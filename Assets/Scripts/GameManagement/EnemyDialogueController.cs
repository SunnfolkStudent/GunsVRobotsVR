using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDialogueController : MonoBehaviour
{
    [Header("Audio and Text")]
    public AudioSource audio;
    public TMP_Text text;
    
    public AudioClip[] audioList;
    public String[] textList;
    
    private void Start()
    {
        
    }
}

