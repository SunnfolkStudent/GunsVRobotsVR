using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _nextLevelTriggerPrefab;
    [SerializeField] private Vector3 _spawnPosition;
    [SerializeField] private GameObject _explosionPrefab;
    
    private void OnTriggerEnter(Collider other)
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity).transform.localScale = new Vector3(2f, 2f, 2f);
        Instantiate(_nextLevelTriggerPrefab, _spawnPosition, Quaternion.identity);
        GameObject.Find("IntroSceneObject").GetComponent<IntroScene>().OnShootEnemy();
        Destroy(gameObject);
    }
}
