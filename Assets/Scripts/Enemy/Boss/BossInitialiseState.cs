using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossInitialiseState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        //Initialise any necessary variables
        boss.currentShield = boss.enemyStats.maxShield;
        boss.currentArmour = boss.enemyStats.maxArmour;
        boss.currentIntegrity = boss.enemyStats.maxIntegrity;
        
        boss.agent = boss.GetComponent<NavMeshAgent>();
        
        boss.agent.speed = boss.enemyStats.moveSpeed;
        
        boss.distanceToPlayer = CalculateDistanceToPlayer(boss);
        
        //Set movement destination
        boss.destination = boss.playerData.position;
        boss.agent.destination = boss.destination;

        boss.SwitchState(boss.MoveTowardsState);
    }

    public override void HandleState(BossStateManager boss)
    {
        
    }
}
