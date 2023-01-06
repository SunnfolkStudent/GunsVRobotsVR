using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyEvadeState : EnemyBaseState
{
    public float maxEvadeDistance = 5f;
    public float evadeSpeed = 5f;

    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.destination = SelectEvadeTarget(enemy);
        enemy.agent.destination = enemy.destination;
        enemy.agent.speed = evadeSpeed;
        
    }

    public override void HandleState(EnemyStateManager enemy)
    {
        if (enemy.agent.remainingDistance > 0.3f)
        {
            return;
        }
        
        if (enemy.distanceToPlayer > enemy.attackRange)
        {
            enemy.agent.speed = enemy.moveSpeed;
            enemy.SwitchState(enemy.MoveTowardsState);
            return;
        }
        else
        {
            enemy.agent.speed = enemy.moveSpeed;
            enemy.SwitchState(enemy.EngageState);
            return;
        }
    }

    private Vector3 SelectEvadeTarget(EnemyStateManager enemy)
    {
        var directionTowardsPlayer = (enemy.playerData.position - enemy.transform.position).normalized;

        var rightOffset = Vector3.Cross(directionTowardsPlayer, Vector3.down).normalized * maxEvadeDistance;
        var rightVector = enemy.transform.position + rightOffset;
        var leftVector = enemy.transform.position + rightOffset;

        var rightNavMeshHit = new NavMeshHit();
        var leftNavMeshHit = new NavMeshHit();
        NavMesh.SamplePosition(rightVector, out rightNavMeshHit, maxEvadeDistance * 0.5f, NavMesh.AllAreas);
        NavMesh.SamplePosition(leftVector, out leftNavMeshHit, maxEvadeDistance * 0.5f, NavMesh.AllAreas);

        var rightMovementVector = rightNavMeshHit.position - enemy.transform.position;
        var leftMovementVector = leftNavMeshHit.position - enemy.transform.position;
        
        var rightPath = new NavMeshPath();
        
        //Checks if a raycast towards the points hits any environment object.
        //Right
        //TODO: Bytt ut med Boxcast?
        var isSightLineBlocked = Physics.Raycast(enemy.transform.position, rightMovementVector.normalized,
            rightMovementVector.magnitude, enemy.whatIsEnvironment);
            
        enemy.agent.CalculatePath(rightNavMeshHit.position, rightPath);
        var isRightValid = rightNavMeshHit.hit && rightPath.status == NavMeshPathStatus.PathComplete && !isSightLineBlocked;
        
        //Left
        isSightLineBlocked = Physics.Raycast(enemy.transform.position, leftMovementVector.normalized,
            leftMovementVector.magnitude, enemy.whatIsEnvironment);
            
        enemy.agent.CalculatePath(rightNavMeshHit.position, rightPath);
        var isLeftValid = rightPath.status == NavMeshPathStatus.PathComplete && !isSightLineBlocked;
        
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
            return enemy.transform.position;
        }
    }
}
