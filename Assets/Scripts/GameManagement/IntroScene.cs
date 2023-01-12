using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : MonoBehaviour
{
    private GameManager _manager;

    private void Update()
    {
        
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
