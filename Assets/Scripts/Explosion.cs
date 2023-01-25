using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Explosion : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, 2f);
        GetComponentInChildren<VisualEffect>().Play();
    }
}
