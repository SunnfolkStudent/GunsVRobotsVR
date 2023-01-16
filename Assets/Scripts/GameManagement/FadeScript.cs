using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeScript : MonoBehaviour
{
   public CanvasGroup myUIGroup;
   private Scene _scene;

    [SerializeField] 
    public bool fadeIn = false;

    [SerializeField] 
    private bool fadeOut = false;

    private GameManager _gameManager;

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
                if (myUIGroup.alpha >= 1)
                { fadeOut = false; }

                /*if (myUIGroup.alpha == 1)
                {
                    SceneManager.LoadScene("TheScene");
                }*/
            }
        }
    }
}
