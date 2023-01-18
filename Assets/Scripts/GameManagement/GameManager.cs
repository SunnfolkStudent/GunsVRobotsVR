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
    
     //[SerializeField]
        //private GameObject _door;
        
    [HideInInspector]
    public int currentMusic = 0;

    private void Awake()
    {
        _fade = GetComponent<FadeScript>();
        
        Debug.Log("Active Scene name is: " + _scene.name + "\nActive Scene index: " + _scene.buildIndex);
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
                    _lineManager.currentMsg = 0;
                    //put nextvoiceline
                    _lineManager.IsFinishedTalking();
                    
                    //TODO: let the voicelines play 
                }
            }
        }

    private void OnAllWavesFinished()
    {
        string sceneName = _scene.name;
        if (EnemyPoolController.CurrentEnemyPoolController.activeEnemies.Count == 0 )
        {
            //TODO: door/area to next level opens
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
    }
    private void OnBossDead()
    {
        //TODO: door/area to next level opens
    }
    
    public void OnNextLevelInteract()
        {
            //when fade alpha is at 1 go to next level
            _fade.ShowUi();
        }
}