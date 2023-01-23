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
        AudioManager.instance.TryRemoveSource(AudioManager.SoundType.Voice, AudioManager.Source.Player, 0);
        AudioManager.instance.TryRemoveSource(AudioManager.SoundType.Sfx, AudioManager.Source.Player, 0);
        switch (SceneManager.GetActiveScene().name)
        {
            case "IntroScene":
            case "Intro_Test":
                GameObject.Find("IntroSceneObject").GetComponent<IntroScene>().LoadNextLevel();
                break;
            default:
                GameObject.Find("GameManager").GetComponent<GameManager>().OnNextLevelInteract();
                break;
        }
    }
}
