using UnityEngine;

public abstract class EnemyBaseState
{
    public abstract void EnterState(EnemyStateManager enemy);

    public abstract void HandleState(EnemyStateManager enemy);
    
    public float CalculateDistanceToPlayer(EnemyStateManager enemy)
    {
        return Vector3.Distance(enemy.transform.position, enemy.playerData.position);
    }
}
