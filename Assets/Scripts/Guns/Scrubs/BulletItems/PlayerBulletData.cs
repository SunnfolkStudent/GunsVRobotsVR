using System;
using UnityEngine;

public class PlayerBulletData : MonoBehaviour
{
    [SerializeField] public GunData gunData;

    public Rigidbody _rigidbody;

    private float startTime;

    private float baseDamageFallOff;
    private float armourPierceFallOff;
    private float armourShredFallOff;
    private float shieldDisruptFallOff;
    private float ShieldPierceFallOff; 

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>(); 
    }

    private void Update()
    {
        if (Time.time >= startTime + gunData.range)
        {
            BulletPoolController.CurrentBulletPoolController.DestroyPlayerBullet(this);
        }
        
        moveBullet();
    }

    private void OnEnable()
    {
        startTime = Time.time; 
    }

    private void moveBullet()
    {
        if (PauseManager.IsPaused)
        {
            _rigidbody.velocity = Vector3.zero;
        }
        else
        {
            _rigidbody.velocity = transform.forward * gunData.bulletSpeed;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Environment/Ground") || col.CompareTag("Environment/SmallObstacle") || col.CompareTag("Environment/LargeObstacle"))
        {
            print("Hit obstacle or ground. " + col.gameObject.name);
            BulletPoolController.CurrentBulletPoolController.DestroyPlayerBullet(this);
        }

        if (col.CompareTag("Enemy"))
        {
            print("Hit enemy. " + col.gameObject.name);
            //initiates damage falloff after bullet has traveled half its range 
            if (Time.time > (startTime + gunData.range / 2))
            {
                //variable checking the time since the bullet was instanciated 
                var timeSinceLaunch = Time.time - startTime;

                if (gunData.fallOff > 0.1f)
                {
                    //variables checking how much damage the bullet should do of each type after damage fall of is calculated
                    baseDamageFallOff = (gunData.BaseDamage * gunData.range) / (gunData.fallOff * timeSinceLaunch);
                    armourPierceFallOff = (gunData.ArmourPierce * gunData.range) / (gunData.fallOff * timeSinceLaunch);
                    armourShredFallOff = (gunData.ArmourShred * gunData.range) / (gunData.fallOff * timeSinceLaunch);
                    ShieldPierceFallOff = (gunData.ShieldPierce * gunData.range) / (gunData.fallOff * timeSinceLaunch);
                    shieldDisruptFallOff = (gunData.ArmourShred * gunData.range) / (gunData.fallOff * timeSinceLaunch);
                }
                else
                {
                    baseDamageFallOff = gunData.BaseDamage;
                    armourPierceFallOff = gunData.ArmourPierce;
                    armourShredFallOff = gunData.ArmourShred;
                    shieldDisruptFallOff = gunData.ShieldDisrupt;
                    ShieldPierceFallOff = gunData.ArmourShred;
                }


                //shieldDisruptFallOff = SetFallOffDamage(gunData.ArmourShred, timeSinceLaunch);
            }

            else

            {
                baseDamageFallOff = gunData.BaseDamage;
                armourPierceFallOff = gunData.ArmourPierce;
                armourShredFallOff = gunData.ArmourShred;
                shieldDisruptFallOff = gunData.ShieldDisrupt;
                ShieldPierceFallOff = gunData.ArmourShred;
            }

            print("damage done" + baseDamageFallOff);

            //feeds enemy information about how much damage it is supposed to take 
            //var enemy = col.GetComponent<EnemyStateManager>();
            if (col.TryGetComponent(out EnemyStateManager enemy))
            {
                if (gunData.isKnockBack)
                {
                    enemy.TakeDamage(baseDamageFallOff, armourPierceFallOff, armourShredFallOff, ShieldPierceFallOff,
                        shieldDisruptFallOff, gunData.KnockBackStun, gunData.KnockBackPush);
                }
                else
                {
                    enemy.TakeDamage(baseDamageFallOff, armourPierceFallOff, armourShredFallOff, ShieldPierceFallOff,
                        shieldDisruptFallOff, 0f, 0f);
                }
            }
            else if (col.TryGetComponent(out SentryBehaviour sentry))
            {
                sentry.TakeDamage(baseDamageFallOff, armourPierceFallOff, armourShredFallOff, ShieldPierceFallOff,
                    shieldDisruptFallOff);
            }
            else if (col.TryGetComponent(out SentryProjectileBehaviour projectile))
            {
                projectile.TakeDamage(baseDamageFallOff, armourPierceFallOff, armourShredFallOff, ShieldPierceFallOff,
                    shieldDisruptFallOff);
            }
            else if (col.TryGetComponent(out Boss.BossStateManager boss))
            {
                boss.TakeDamage(baseDamageFallOff, armourPierceFallOff, armourShredFallOff, ShieldPierceFallOff,
                    shieldDisruptFallOff, 0f, 0f);
            }
            
            
            
            
            BulletPoolController.CurrentBulletPoolController.DestroyPlayerBullet(this);
        }
            
    }

    /*float SetFallOffDamage(float damageType, float timeSinceLaunch)
    {
        return damageType - (gunData.fallOff * (timeSinceLaunch - (gunData.range / 2)));
    }*/
}
