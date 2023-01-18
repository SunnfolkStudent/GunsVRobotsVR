using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IntroEnemyController : MonoBehaviour
{
    [SerializeField] private IntroScene _introScene;

    private void OnTriggerEnter(Collider other)
    {
        _introScene.OnShootEnemy();
    }
}
