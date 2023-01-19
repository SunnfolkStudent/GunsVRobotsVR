using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class MoveMeRandom : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float forceAmount = 2;
    private float maxVelocity = 10;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        _rigidbody.AddForce(Vector3.forward * forceAmount, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        if (_rigidbody.velocity.magnitude > maxVelocity)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * maxVelocity;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Environment/Ground"))
        {
            float force = 300;
            
            Vector3 direction = other.contacts[0].point - transform.position;          
            direction = -direction.normalized;
            GetComponent<Rigidbody>().AddForce(direction * force);
        }
        else if (other.gameObject.CompareTag("Environment/LargeObstacle"))
        {
            float force = 100;
            
            Vector3 direction = other.contacts[0].point - transform.position;          
            direction = -direction.normalized;
            GetComponent<Rigidbody>().AddForce(direction * force);
        }
    }
}
