using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End_Screen : MonoBehaviour
{
    private FadeScript _fade;
    
    private void OnFadeFinished()
    {
        if ( _fade.myUIGroup.alpha == 0)
        {
            //TODO: play a voice line
            //TODO: when finished start credits and sunset
        }
    }
    private void OnCreditsFinished()
    {
        _fade.ShowUi();
        if (_fade.myUIGroup.alpha == 1)
        {
            //TODO: Reload the game, go back to start
        }
    }
}
