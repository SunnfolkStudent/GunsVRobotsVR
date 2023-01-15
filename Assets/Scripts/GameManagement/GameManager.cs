using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   //DO NOT PUT THIS IN INTROSCENE 
    
    public static GameManager Instance;
    
    private FadeScript _fade;
    private DialogLineManager _lineManager;
    private IntroScene _intro;

    private AudioSource musicPlayer;
    private AudioClip[] musicList; 
    [HideInInspector]
    public int currentMusic = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        _fade.HideUi();
        
        currentMusic = musicList.Length;
        musicPlayer.PlayOneShot(musicList[currentMusic]);
    }

    private void Update()
    {
        OnFadeFinished();
    }

    private void OnFadeFinished()
        { 
            Scene currentScene = SceneManager.GetActiveScene ();
            string sceneName = currentScene.name;
            
            if ( _fade.myUIGroup.alpha == 0)
            {
                _lineManager.IsTalking();
                if (sceneName == "Arena_1")
                {
                    _lineManager.currentMsg = 3;
                    _lineManager.NextVoiceLine();
                    _lineManager.IsFinishedTalking();
                }
                else if (sceneName == "Arena_2")
                {
                    
                }
                else if (sceneName == "Boss_Arena")
                {
                    //TODO: let the voicelines play 
                    //TODO: when the audio is finished playing, start the infinite waves
                    //TODO: spawn teh boss
                }
            }
        }

    private void OnAllWavesFinished()
    {
        Scene currentScene = SceneManager.GetActiveScene ();
        string sceneName = currentScene.name;
        //TODO: if enemy or wave count is 0 play voice line
        {
            _lineManager.IsTalking();
            if (sceneName == "Arena_1")
            {
                _lineManager.currentMsg = 5;
                _lineManager.NextVoiceLine();
                _lineManager.IsFinishedTalking();
            }
            else if (sceneName == "Arena_2")
            {
                
            }
        }
        
        //TODO: door becomes available to go to next level
        currentMusic = 2;
    }

    private void OnBossDead()
    {
        Scene currentScene = SceneManager.GetActiveScene ();
        string sceneName = currentScene.name;
        
        if (sceneName == "Boss")
        {
            
        }
    }
    
    private void OnNextLevelInteract()
        {
            //if()//TODO: click on door
            {_fade.ShowUi();}
            
            //when fade alpha is at 1 go to next level
            if ( _fade.myUIGroup.alpha == 1)
            {
                SceneManager.LoadScene(sceneBuildIndex: + 1);
            }
        }
}