using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    
    private void Update()
    {
        transform.LookAt(new Vector3(_playerData.position.x, transform.position.y, _playerData.position.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerBulletData>(out var bullet)) return;
        
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
