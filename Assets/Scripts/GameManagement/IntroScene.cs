using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : MonoBehaviour
{
    private GameManager _manager;
    private DialogLineManager _lineManager;

    public void Update()
    {
        OnPlay();
    }

    public void OnPlay()
    {
        _lineManager.currentMsg = 1;
        _lineManager.isTalking = true;
    }
    
    private void OnShootEnemy()
    {
        //TODO: when enemy has been shoot play voiceline
        //TODO: play music 
        //TODO: logo starts fading in 
    }

    private void OnTimerFinished()
    {
        //TODO: when logo has stopped fading and players VL has finished
        //TODO: fade to black and load next scene
    }
}
