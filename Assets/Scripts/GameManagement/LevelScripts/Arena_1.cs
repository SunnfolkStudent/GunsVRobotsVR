using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public class Arena_1 : MonoBehaviour
{
    private GameManager _manager;
    private DialogLineManager _lineManager;
    private FadeScript _fade;

    private void OnFadeFinished()
    {
        if (_fade.myUIGroup.alpha == 0)
        {
            _lineManager.currentMsg = 1;
            _lineManager.isTalking = true;

            if (_lineManager.isTalking == false)
            {
                //TODO: start spawning waves and enemies
            }
        }
    }

    private void OnAllWavesFinished()
    {
        //if (no more enemies)
        {
             _lineManager.currentMsg = 1;
                    _lineManager.isTalking = true;
        }
    }
}
