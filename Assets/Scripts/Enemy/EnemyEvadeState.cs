using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyEvadeState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.destination = SelectEvadeTarget(enemy);
        enemy.agent.destination = enemy.destination;
        enemy.agent.speed = enemy.enemyStats.evadeSpeed;
        
    }

    public override void HandleState(EnemyStateManager enemy)
    {
        if (enemy.agent.remainingDistance > 0.3f)
        {
            return;
        }

        enemy.distanceToPlayer = CalculateDistanceToPlayer(enemy);
        
        if (enemy.distanceToPlayer > enemy.enemyStats.attackRange)
        {
            enemy.agent.speed = enemy.enemyStats.moveSpeed;
            enemy.SwitchState(enemy.MoveTowardsState);
            return;
        }
        else
        {
            enemy.agent.speed = enemy.enemyStats.moveSpeed;
            enemy.SwitchState(enemy.EngageState);
            return;
        }
    }

    private Vector3 SelectEvadeTarget(EnemyStateManager enemy)
    {
        var directionTowardsPlayer = (enemy.playerData.position - enemy.transform.position).normalized;

        var rightOffset = Vector3.Cross(directionTowardsPlayer, Vector3.down).normalized * enemy.enemyStats.maxEvadeDistance;
        var rightVector = enemy.transform.position + rightOffset;
        var leftVector = enemy.transform.position + rightOffset;

        var rightNavMeshHit = new NavMeshHit();
        var leftNavMeshHit = new NavMeshHit();
        NavMesh.SamplePosition(rightVector, out rightNavMeshHit, enemy.enemyStats.maxEvadeDistance * 0.5f, NavMesh.AllAreas);
        NavMesh.SamplePosition(leftVector, out leftNavMeshHit, enemy.enemyStats.maxEvadeDistance * 0.5f, NavMesh.AllAreas);

        var rightMovementVector = rightNavMeshHit.position - enemy.transform.position;
        var leftMovementVector = leftNavMeshHit.position - enemy.transform.position;
        
        var rightPath = new NavMeshPath();

        var isPathBlocked = true;
        
        //Checks if the path towards the points is clear.
        //Right
        var isRightValid = false;
        if (rightNavMeshHit.hit)
        {
            isPathBlocked = Physics.BoxCast(enemy.transform.position, 
                enemy.GetComponent<CapsuleCollider>().bounds.extents, rightMovementVector.normalized,
                enemy.transform.rotation, enemy.distanceToPlayer, enemy.whatIsEnvironment);
            
            enemy.agent.CalculatePath(rightNavMeshHit.position, rightPath);
            isRightValid = rightNavMeshHit.hit && rightPath.status == NavMeshPathStatus.PathComplete && !isPathBlocked;
        }
        
        //Left
        var isLeftValid = false;
        if (leftNavMeshHit.hit)
        {
            isPathBlocked = Physics.BoxCast(enemy.transform.position,
                enemy.GetComponent<CapsuleCollider>().bounds.extents, leftMovementVector.normalized,
                enemy.transform.rotation, enemy.distanceToPlayer, enemy.whatIsEnvironment);
            
            enemy.agent.CalculatePath(rightNavMeshHit.position, rightPath);
            isLeftValid = rightPath.status == NavMeshPathStatus.PathComplete && !isPathBlocked;
        }
        
        
        //Returns the best point
        if (isRightValid && isLeftValid)
        {
            if (rightNavMeshHit.distance < leftNavMeshHit.distance)
            {
                return rightNavMeshHit.position;
            }
            else
            {
                return leftNavMeshHit.position;
            }
        }
        else if (isRightValid)
        {
            return rightNavMeshHit.position;
        }
        else if (isLeftValid)
        {
            return leftNavMeshHit.position;
        }
        else
        {
            Debug.Log("Enemy failed to find evade-target.");
            return enemy.transform.position;
        }
    }
}