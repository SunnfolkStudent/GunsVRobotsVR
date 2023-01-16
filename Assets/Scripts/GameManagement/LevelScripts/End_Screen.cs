using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End_Screen : MonoBehaviour
{
    private GameManager _manager;
    private FadeScript _fade;
    private DialogLineManager _lineManager;
    
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
        _fade.ShowUi();
        if (_fade.myUIGroup.alpha == 1)
        {
            SceneManager.LoadScene(sceneName:"Intro");
        }
    }
}
