using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyDialogueController : MonoBehaviour
{
    [Header("Audio and Text")]
    public AudioSource enemyAudio;
    public TMP_Text enemyText;
    
    public AudioClip[] enemyAudioList;
    public String[] EnemyTextList;

    private int randomIndex;
    
    private void Start()
    {
        enemyAudio = GetComponentInChildren<AudioSource>();
        enemyText = GetComponent<TMP_Text>();
    }

    private void EnemyIsTalking()
    {
        randomIndex = Random.Range(0,EnemyTextList.Length);
                    enemyText.text = EnemyTextList[randomIndex];
                    enemyAudio.PlayOneShot(enemyAudioList[randomIndex]);
    }
}

