using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   //DO NOT PUT THIS IN INTRO SCENE OR END SCREEN
    
    private FadeScript _fade;
    private DialogLineManager _lineManager;
    private IntroScene _intro;
    private Scene _scene;
    
    [SerializeField]
    private AudioSource _musicPlayer;
    [SerializeField]
    private AudioClip[] _musicList;
    [SerializeField]
    private int enemiesLeft;
            
     //[SerializeField]
        //private GameObject _door;
        
    [HideInInspector]
    public int currentMusic = 0;

    private void Awake()
    {
        _fade = GetComponent<FadeScript>();
        
        Debug.Log("Active Scene name is: " + _scene.name + "\nActive Scene index: " + _scene.buildIndex);
        
        currentMusic = _musicList.Length;
        //musicPlayer.PlayOneShot(musicList[currentMusic]);
    }

    private void Start()
    { _fade.HideUi(); }
    private void Update()
    { OnFadeFinished(); }
    

    private void OnFadeFinished()
        { 
            string sceneName = _scene.name;
            
            if ( _fade.myUIGroup.alpha == 0)
            {
                //_lineManager.IsTalking();

                
                if (sceneName == "Arena_1")
                {
                    /*_lineManager.currentMsg = 3;
                    _lineManager.NextVoiceLine();
                    _lineManager.IsFinishedTalking();*/

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
        string sceneName = _scene.name;
        //TODO: if enemy or wave count is 0 play voice line
        {
            _lineManager.IsTalking();
            if (sceneName == "Arena_1")
            {
                /*_lineManager.currentMsg = 5;
                _lineManager.NextVoiceLine();
                _lineManager.IsFinishedTalking();*/
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
    
    public void OnNextLevelInteract()
        {
           
            //if()//TODO: click on door
            
            //when fade alpha is at 1 go to next level
            
            
        }

    
}