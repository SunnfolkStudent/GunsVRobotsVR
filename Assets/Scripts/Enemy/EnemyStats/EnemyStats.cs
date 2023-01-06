using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Info")]
    public string enemyName;
    public GameObject enemyModel;
    
    [Header("Health")]
    public int maxShield = 100;
    public int maxArmour = 100;
    public int maxIntegrity = 100;
    
    [Space (10)]
    [Header("Movement")]
    public float moveSpeed = 0f;
    public float attackRange = 0f;
    
    //[Header("Gun")]
    //public GunData gunType;
}
