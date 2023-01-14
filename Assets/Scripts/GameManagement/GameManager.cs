using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    
    private FadeScript _fade;
    private DialogLineManager _lineManager;
    private IntroScene _intro;

    private void Awake()
    {
        _fade.HideUi();
        
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
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
                if (sceneName == "Intro")
                {
                    _lineManager.isTalking = false;
                }
                else if (sceneName == "Arena_1")
                {
                    _lineManager.currentMsg = 3;
                    _lineManager.currentMsg = +1;
                }
                else if (sceneName == "Arena_2")
                {
                    
                }
                else if (sceneName == "Boss")
                {
                    
                }
                else if (sceneName == "EndScreen")
                {
                    
                }
            }
        }

    private void OnAllWavesFinished()
    {
        //TODO: if enemy or wave count is 0 play voice line
        //TODO: door becomes available to go to next level
        //TODO: switch music
    }
    
    private void OnNextLevelInteract()
        {
            //if()//TODO: click on door
            {_fade.ShowUi();}
            
            //when fade alpha is at 1 go to next level
            if ( _fade.myUIGroup.alpha == 1)
            {
                SceneManager.LoadScene(sceneBuildIndex: +1);
            }
        }
}