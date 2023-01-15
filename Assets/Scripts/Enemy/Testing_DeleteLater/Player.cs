using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerData _playerData;

    private void Update()
    {
        _playerData.position = transform.position;
    }
}
