using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Range = UnityEngine.SocialPlatforms.Range;

public class IntroScene : MonoBehaviour
{
    private GameManager _manager;
    private DialogLineManager _lineManager;
    private FadeScript _fade;
    private AudioSource _audioSource;
    private Scene _scene;
    
    [SerializeField]
    private Animator _animator;
    [SerializeField] 
    private AudioClip[] _MusicClips;
    [SerializeField]
    private GameObject settingsCanvas;

    public Animation Sunset;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _fade = GetComponent<FadeScript>();
        _animator = GetComponent<Animator>();

        //TODO: Play loop music :)

        // _audioSource.Play(_MusicClips);
    }
    
    public void OnPlay()
    {
        settingsCanvas.SetActive(false);
        
        _lineManager.currentMsg = 1;
        _lineManager.isTalking = true;
    }
    
    private void OnShootEnemy()
    {
        //TODO: when shot start voice line and music, or play voiceline before music kicks off?
        _lineManager.currentMsg = 1;
        _lineManager.isTalking = true;
        
        //TODO: play music 
        _manager.currentMusic = 1;
        //TODO: logo starts fading in 
    }

    public void OnTimerFinished()
    {
        Debug.Log("intro is loading next scene");
        //TODO: when logo has stopped fading and players VL has finished, with a timer?
        //TODO: fade to black and load next scene

        //if (when logo is done showing for a certain amount of time, screen fade)
        {
            _fade.ShowUi();
            //when fade alpha is at 1 go to next level
        }
    }
}
