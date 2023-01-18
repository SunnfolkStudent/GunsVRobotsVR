using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End_Screen : MonoBehaviour
{
    private GameManager _manager;
    private FadeScript _fade;
    private AudioSource _audioSource;
    private DialogLineManager _lineManager;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _fade = GetComponent<FadeScript>();
        
        _fade.HideUi();
    }

    private void OnFadeFinished()
    {
        if ( _fade.myUIGroup.alpha == 0)
        {
            _lineManager.currentMsg = 9;
            if (_lineManager.isTalking == false)
            {
                //TODO: when finished start credits and sunset
            }
        }
    }
    private void OnCreditsFinished()
    {
        //TODO: when credits and sunset are finished get back to the intro scene
        _fade.ShowUi();
    }
}
