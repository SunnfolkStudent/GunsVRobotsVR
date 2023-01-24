using System;
using UnityEngine;

public class EnemyBulletData : MonoBehaviour
{
    [SerializeField] public GunData gunData;

    public Rigidbody _rigidbody;
    
    private float startTime;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>(); 
    }

    private void Update()
    {
        if (Time.time >= startTime + gunData.range)
        {
            BulletPoolController.CurrentBulletPoolController.RegisterEnemyBulletAsInactive(this);
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
            BulletPoolController.CurrentBulletPoolController.RegisterEnemyBulletAsInactive(this);
        }

        if (col.CompareTag("Player"))
        {
            var player = col.GetComponentInParent<PlayerHealthManager>();
            player.TakeDamage(gunData.BaseDamage, gunData.ArmourPierce, gunData.ArmourShred, gunData.ShieldPierce,
                gunData.ShieldDisrupt);


            BulletPoolController.CurrentBulletPoolController.RegisterEnemyBulletAsInactive(this);
        }
            
    }
}