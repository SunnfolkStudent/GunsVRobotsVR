using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _nextLevelTriggerPrefab;
    [SerializeField] private Vector3 _spawnPosition;
    
    private void OnTriggerEnter(Collider other)
    {
        Instantiate(_nextLevelTriggerPrefab, _spawnPosition, Quaternion.identity);
        GameObject.Find("IntroSceneObject").GetComponent<IntroScene>().OnShootEnemy();
    }
}
