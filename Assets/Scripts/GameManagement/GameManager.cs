using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private FadeScript _fade;

    private void Awake()
    {
        _fade.HideUi();
    }

    private void Update()
    {
        OnFadeFinished();
    }

    private void PlayVoiceLine()
    {
        
    }
    
    private void OnFadeFinished()
        {
            if (_fade.myUIGroup.alpha == 0)
            {
                //TODO: play specific voicelines
            }
        }

    private void OnAllWavesFinished()
    {
        //TODO: if enemy or wave count is 0 play voice line
        //TODO: door becomes available to go to next level
    }
    
    private void OnNextLevelInteract()
        {
            //TODO: click on door
            _fade.ShowUi();
            //TODO: when fade alpha is at 1 go to next level
        }
}