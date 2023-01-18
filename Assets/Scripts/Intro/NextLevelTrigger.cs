using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Intro":
            case "Intro_Test":
                GameObject.Find("GameObject").GetComponent<IntroScene>().OnShootEnemy();
                break;
            default:
                GameObject.Find("GameManager").GetComponent<GameManager>().OnNextLevelInteract();
                break;
        }
    }
}
