using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class BossIdleState : BossBaseState
    {
        private float _checkForPlayerTime;
        private bool _strafeLeft;

        public override void EnterState(BossStateManager boss)
        {
            _checkForPlayerTime = Time.time + boss.minimumTimeInIdleState;
            boss.animator.Play("Boss_Idle");
        }

        public override void HandleState(BossStateManager boss)
        {
            if (boss.HasCrossedDamageThreshold())
            {
                boss.SwitchState(boss.ShieldState);
                return;
            }
            
            boss.transform.LookAt(
                new Vector3(boss.playerData.position.x, boss.transform.position.y, boss.playerData.position.z));
            
            var directionTowardsPlayer = boss.playerData.position - boss.transform.position;
            directionTowardsPlayer.y = 0f;
            var distanceToPlayer = directionTowardsPlayer.magnitude;
            directionTowardsPlayer = directionTowardsPlayer.normalized;
            
            IdleMovement(boss, directionTowardsPlayer, distanceToPlayer);

            if (Time.time <= _checkForPlayerTime) return;

            var isSightLineBlocked = Physics.Raycast(boss.playerData.position, -directionTowardsPlayer,
                distanceToPlayer, boss.whatIsEnvironment);
            
            if (isSightLineBlocked) return;
            
            boss.SwitchState(boss.ShootState);
        }

        private void IdleMovement(BossStateManager boss, Vector3 directionTowardsPlayer, float distanceToPlayer)
        {
            if (distanceToPlayer > boss.maxDistanceToPlayer)
            {
                if (!HasSomethingInFront(boss, directionTowardsPlayer))
                {
                    boss.rb.velocity = directionTowardsPlayer * boss.movementSpeed;
                    return;
                }
            }
            else if (distanceToPlayer < boss.minDistanceToPlayer)
            {
                if (!HasSomethingBehind(boss, directionTowardsPlayer))
                {
                    boss.rb.velocity = -directionTowardsPlayer * boss.movementSpeed;
                    return;
                }
            }
            
            //Strafe
            if (_strafeLeft)
            {
                if (HasSomethingOnTheLeft(boss, directionTowardsPlayer))
                {
                    _strafeLeft = false;
                    boss.rb.velocity = boss.movementSpeed * (Quaternion.Euler(0f, 90f, 0f) * directionTowardsPlayer);
                }
                else
                {
                    boss.rb.velocity = boss.movementSpeed * (Quaternion.Euler(0f, -90f, 0f) * directionTowardsPlayer);
                }
            }
            else
            {
                if (HasSomethingOnTheRight(boss, directionTowardsPlayer))
                {
                    _strafeLeft = true;
                    boss.rb.velocity = boss.movementSpeed * (Quaternion.Euler(0f, -90f, 0f) * directionTowardsPlayer);
                }
                else
                {
                    boss.rb.velocity = boss.movementSpeed * (Quaternion.Euler(0f, 90f, 0f) * directionTowardsPlayer);
                }
            }
        }

        private bool HasSomethingInFront(BossStateManager boss, Vector3 directionTowardsPlayer)
        {
            return Physics.Raycast(boss.transform.position, directionTowardsPlayer.normalized,
                boss.maxIdleRaycastDistance, boss.whatIsEnvironment);
        }
        
        private bool HasSomethingBehind(BossStateManager boss, Vector3 directionTowardsPlayer)
        {
            return Physics.Raycast(boss.transform.position, -directionTowardsPlayer.normalized,
                boss.maxIdleRaycastDistance, boss.whatIsEnvironment);
        }

        private bool HasSomethingOnTheLeft(BossStateManager boss, Vector3 directionTowardsPlayer)
        {
            return Physics.Raycast(boss.transform.position,
                Quaternion.Euler(0f, -90f, 0f) * directionTowardsPlayer.normalized,
                boss.maxIdleRaycastDistance, boss.whatIsEnvironment);
        }
        
        private bool HasSomethingOnTheRight(BossStateManager boss, Vector3 directionTowardsPlayer)
        {
            return Physics.Raycast(boss.transform.position,
                Quaternion.Euler(0f, 90f, 0f) * directionTowardsPlayer.normalized,
                boss.maxIdleRaycastDistance, boss.whatIsEnvironment);
        }
    }
}