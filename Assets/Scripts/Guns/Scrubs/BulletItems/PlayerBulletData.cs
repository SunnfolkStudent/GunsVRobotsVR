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
        if (col.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

        if (col.CompareTag("Enemy"))
        {
            //initiates damage falloff after bullet has traveled half its range 
            if (Time.time > (startTime + gunData.range / 2))
            {
                //variable checking the time since the bullet was instanciated 
                var timeSinceLaunch = Time.time - startTime;
                
                //variables checking how much damage the bullet should do of each type after damage fall of is calculated
                baseDamageFallOff = (gunData.BaseDamage - (gunData.fallOff * (timeSinceLaunch - (gunData.range / 2))));
                armourPierceFallOff = (gunData.ArmourPierce - (gunData.fallOff * (timeSinceLaunch - (gunData.range / 2))));
                armourShredFallOff = (gunData.ArmourShred - (gunData.fallOff * (timeSinceLaunch - (gunData.range / 2))));
                ShieldPierceFallOff = (gunData.ShieldPierce - (gunData.fallOff * (timeSinceLaunch - (gunData.range / 2))));
                shieldDisruptFallOff = (gunData.ArmourShred - (gunData.fallOff * (timeSinceLaunch - (gunData.range / 2))));
               
                
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

            //feeds enemy information about how much damage it is supposed to take 
            var enemy = col.GetComponent<EnemyHitdetection>();
            enemy.TakeDamage(baseDamageFallOff, armourPierceFallOff, armourShredFallOff, ShieldPierceFallOff,
                shieldDisruptFallOff);
            
            Destroy(gameObject);
        }
            
    }

    /*float SetFallOffDamage(float damageType, float timeSinceLaunch)
    {
        return damageType - (gunData.fallOff * (timeSinceLaunch - (gunData.range / 2)));
    }*/
}
