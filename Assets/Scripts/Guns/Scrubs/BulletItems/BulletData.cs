using System;
using UnityEngine;

public class BulletData : MonoBehaviour
{
    [SerializeField] public GunData gunData;

    public Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>(); 
    }

    private void Update()
    {
        moveBullet();
    }

    private void moveBullet()
    {
        _rigidbody.velocity = transform.forward * gunData.bulletSpeed;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

        if (col.CompareTag("Enemy"))
        {
            var enemy = col.GetComponent<EnemyHitdetection>();
            enemy.TakeDamage(gunData.BaseDamage, gunData.ArmourPierce, gunData.ArmourShred, gunData.ShieldPierce,
                gunData.ShieldDisrupt);


            Destroy(gameObject);
        }
            
    }
}
