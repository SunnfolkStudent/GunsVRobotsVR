using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCollision : MonoBehaviour
{
    public void AmIPickedUp(bool isPoweredUp)
    {
        
        //checks if we're allowed to pick up powerup 
        if (!isPoweredUp)
        {
            Destroy(gameObject);
        }
    }
}
