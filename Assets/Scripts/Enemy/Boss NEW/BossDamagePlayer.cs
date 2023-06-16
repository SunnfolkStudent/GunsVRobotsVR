using UnityEngine;

public class BossDamagePlayer : MonoBehaviour
{
    public Boss.BossStateManager boss;
    
    private void OnTriggerEnter(Collider col)
    {
        if (boss.currentState != boss.ChargeState) return;
        
        if (col.CompareTag("Player"))
        {
            var player = col.GetComponentInParent<PlayerHealthManager>();
            player.TakeDamage(boss.chargeDamage * 0.5f, 0f, 0f, 0f,
                0f);
        }
        else if (col.CompareTag("Environment/LargeObstacle"))
        {
            boss.SwitchState(boss.FallState);
        }
    }
}
