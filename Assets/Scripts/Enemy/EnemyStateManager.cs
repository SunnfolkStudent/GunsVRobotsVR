using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

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
    
    public float currentShield;
    public float currentArmour;
    public float currentIntegrity;

    public LayerMask whatIsEnvironment;
    public NavMeshAgent agent;
    public Vector3 destination;

    public PlayerData playerData;
    public float distanceToPlayer;

    public GameObject[] lootDrops;
    public GameObject[] healthDrops;

    public int itemNum;

    private void Update()
    {
        if (PauseManager.IsPaused) return;

        if (currentIntegrity <= 0)
        {
            SwitchState(DeathState);
            return;
        }

        //Rotate towards player, but keep up-direction
        var directionTowardsPlayer = playerData.position - transform.position;
        directionTowardsPlayer = directionTowardsPlayer - new Vector3(0f, directionTowardsPlayer.y, 0f);
        directionTowardsPlayer = directionTowardsPlayer.normalized;

        transform.Rotate(0, Vector3.Angle(transform.forward, directionTowardsPlayer), 0);

        currentState.HandleState(this);
    }

    public void SwitchState(EnemyBaseState newState)
    {
        if (currentState == DeathState)
        {
            return;
        }
        currentState = newState;
        currentState.EnterState(this);
    }
}
