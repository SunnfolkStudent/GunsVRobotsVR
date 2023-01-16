using UnityEngine;

public class EnemyKnockBackState : EnemyBaseState
{
    private float _knockBackStartTime;
    
    public EnemyBaseState stateToReturnTo;
    public float stunTime;
    public float knockBackForce;
    public Vector3 knockBackDirection;
    

    public override void EnterState(EnemyStateManager enemy)
    {
        _knockBackStartTime = Time.time;
        enemy.animator.enabled = false;
        enemy.agent.enabled = false;
    }

    public override void HandleState(EnemyStateManager enemy)
    {
        var knockBackMagnitude = Mathf.Lerp(knockBackForce, 0.1f, (Time.time - _knockBackStartTime) / stunTime);
        enemy.transform.Translate(knockBackMagnitude * knockBackDirection);

        if ((Time.time - _knockBackStartTime) / stunTime >= 1f)
        {
            enemy.animator.enabled = true;
            enemy.agent.enabled = false;
            enemy.currentState = stateToReturnTo;
        }
    }
}
