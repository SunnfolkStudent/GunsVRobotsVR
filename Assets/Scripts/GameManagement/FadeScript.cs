using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeScript : MonoBehaviour
{
   public CanvasGroup myUIGroup;
   public bool fadeIn = false; 
   public bool fadeOut = false;

    private GameManager _gameManager;
    private Scene _scene;
    
    private void Awake()
    {
        _gameManager = GetComponent<GameManager>();
        _scene = SceneManager.GetActiveScene();
        
        HideUi();
    }

    public void ShowUi()
    {
        fadeIn = true;
        fadeOut = false;
        Debug.Log("Showing");
    }

    public void HideUi()
    {
        if (fadeOut == false)
        {
            fadeOut = true;
        }
        Debug.Log("Hiding");
    }
    
    private void Update()
    {
        string sceneName = _scene.name;
        if (fadeOut)
        {
            if (myUIGroup.alpha >= 0)
            {
                myUIGroup.alpha -= Time.deltaTime;
                if (myUIGroup.alpha == 0)
                { fadeIn = false; }
            }
        }

        if (fadeIn)
        {
            if (myUIGroup.alpha < 1)
            {
                myUIGroup.alpha += Time.deltaTime;
                 if (myUIGroup.alpha >= 1 && sceneName == "EndScreen")
                {
                    SceneManager.LoadScene(sceneName:"Intro");
                    Debug.Log("Next scene Loaded");
                }
                else if (myUIGroup.alpha >= 1)
                {
                    fadeOut = false; 
                    LoadScene();
                }
            }
        }
    }
    public void LoadScene()
    {
        int sceneIndex = _scene.buildIndex;
        
        SceneManager.LoadScene(sceneIndex + 1);
        Debug.Log("Next scene Loaded");
    }
}
