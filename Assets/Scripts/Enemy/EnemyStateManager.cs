using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    public EnemyBaseState currentState;
    public EnemyInitialiseState InitialiseState = new EnemyInitialiseState();
    public EnemyMoveTowardsState MoveTowardsState = new EnemyMoveTowardsState();
    public EnemyEngageState EngageState = new EnemyEngageState();
    public EnemyPerformAttackState PerformAttackState = new EnemyPerformAttackState();
    public EnemyEvadeState EvadeState = new EnemyEvadeState();
    public EnemyDeathState DeathState = new EnemyDeathState();

    public EnemyStats enemyStats;
    
    public int currentShield;
    public int currentArmour;
    public int currentIntegrity;

    public LayerMask whatIsEnvironment;
    public NavMeshAgent agent;
    public Vector3 destination;

    public PlayerData playerData;
    public float distanceToPlayer;
    

    public void Awake()
    {
        currentState = InitialiseState;
        InitialiseState.EnterState(this);
    }

    private void Update()
    {
        currentState.HandleState(this);
    }

    public void SwitchState(EnemyBaseState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }
    
    
}

public class PlayerData
{
    public Vector3 position;
}
