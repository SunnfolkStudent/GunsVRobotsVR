using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitdetection : MonoBehaviour
{
    [SerializeField] private EnemyStats enemyStats;

    public float Integrity;
    public float Armour;
    public float Shield;
    

    private void Start()
    {
        Integrity = enemyStats.maxIntegrity;
        Armour = enemyStats.maxArmour;
        Shield = enemyStats.maxShield;
    }

    private void Update()
    {
        if (Integrity <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float dmg, float armourPierce, float armourShred, float shieldPierce, float shieldDisrupt)
    {
        print("I got hit today");
        
        if (Shield >= 0)
        {
            Shield -= ((dmg + armourPierce + armourShred + armourPierce) / 2 + shieldDisrupt);

            if (Armour > 0)
            {
                Armour -= shieldPierce;
            }
            
            else
            {
                Integrity -= shieldPierce;
            }
        }

        if (Shield <= 0 && Armour >= 0)
        {
            Armour -= ((dmg + armourPierce + shieldPierce + shieldDisrupt) / 2 + armourShred);
            Integrity -= armourPierce;
        }

        if (Shield <= 0 && Armour <= 0)
        {
            Integrity -= (dmg + armourPierce + shieldPierce + shieldDisrupt + armourShred + armourPierce) / 2;
        }
    }
}
