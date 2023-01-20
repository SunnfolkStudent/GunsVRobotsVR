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
    private Scene _scene;
    
    [SerializeField]
    private GameObject settingsCanvas;
    [SerializeField] 
    private LerpPos logo;
    
    public bool test;

    private void Start()
    {
        _fade = GetComponent<FadeScript>();
    }
    
    private void Update()
    {
        if (test)
        {
            test = false;
            OnShootEnemy();
        }
    }
    
    public void OnPlay()
    {
        settingsCanvas.SetActive(false);
    }
    
    public void OnShootEnemy()
    {
        AudioManager.instance.fmodManager.SetGameState(FMODMusicManager.GameState.Menu);
        logo.Spawn();
        StartCoroutine(CheckIfFinished());
    }

    public IEnumerator CheckIfFinished()
    {
        yield return new WaitForSeconds(9f);
        
        OnTimerFinished();
    }

    public void OnTimerFinished()
    {
        Debug.Log("intro is loading next scene");
        _fade.ShowUi();
    }
}
    