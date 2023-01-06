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
}
