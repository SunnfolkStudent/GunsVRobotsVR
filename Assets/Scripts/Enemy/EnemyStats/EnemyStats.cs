using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Info")]
    public string category;
    public GameObject enemyModel;
    
    [Header("Health")]
    public float maxShield = 100f;
    public float maxArmour = 100f;
    public float maxIntegrity = 100f;
    
    [Space (10)]
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float engageSpeed = 5f;
    public float evadeSpeed = 5f;
    public float evadeDelay = 1f;
    public float evasionChance = 0.7f;
    public float maxEvadeDistance = 5f;

    [Header("Attack")]
    public float attackRange = 5f;
    public float attackDelay = 5f;
    public GunData gunData;

    [Header("Animation")]
    public RuntimeAnimatorController AnimatorController;
}