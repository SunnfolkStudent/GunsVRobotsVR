using System;
using UnityEngine;

public class EnemyBulletData : MonoBehaviour
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

        if (col.CompareTag("Player"))
        {
            var player = col.GetComponent<PlayerHealthManager>();
            player.TakeDamage(gunData.BaseDamage, gunData.ArmourPierce, gunData.ArmourShred, gunData.ShieldPierce,
                gunData.ShieldDisrupt);


            Destroy(gameObject);
        }
            
    }
}