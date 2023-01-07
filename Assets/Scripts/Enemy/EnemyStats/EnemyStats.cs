using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Info")]
    public string enemyName;
    public GameObject enemyModel;
    
    [Header("Health")]
    public float maxShield = 100f;
    public float maxArmour = 100f;
    public float maxIntegrity = 100f;
    
    [Space (10)]
    [Header("Movement")]
    public float moveSpeed = 0f;
    
    public float evadeSpeed = 0f;
    public float maxEvadeDistance = 0f;

    [Header("Attack")]
    public float attackRange = 0f;
    public float attackDelay = 0f;
    public GunData gunData;
}
