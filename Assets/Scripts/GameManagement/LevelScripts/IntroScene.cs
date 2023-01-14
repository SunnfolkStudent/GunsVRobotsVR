using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Range = UnityEngine.SocialPlatforms.Range;

public class IntroScene : MonoBehaviour
{
    private GameManager _manager;
    private DialogLineManager _lineManager;
    private FadeScript _fade;

    private AudioSource _audioSource;
    [SerializeField] 
    private AudioClip[] _MusicClips;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        //TODO: Play loop music :)
        
       // _audioSource.Play(_MusicClips);
    }

    public void Update()
    {
        
    }

    public void OnPlay()
    {
        //TODO: deactivate menu game object
        _lineManager.currentMsg = 1;
        _lineManager.isTalking = true;
    }
    
    private void OnShootEnemy()
    {
        _lineManager.currentMsg = 1;
        _lineManager.isTalking = true;
        
        //TODO: play music 
        //TODO: logo starts fading in 
    }

    private void OnTimerFinished()
    {
        //TODO: when logo has stopped fading and players VL has finished
        //TODO: fade to black and load next scene
        
        //if (when logo is done showing for a certain amount of time, screen fade)
        {
            _fade.ShowUi();
            
            //when fade alpha is at 1 go to next level
            if ( _fade.myUIGroup.alpha == 1)
            {
                SceneManager.LoadScene(String.Empty);
            }
        }
    }
}
