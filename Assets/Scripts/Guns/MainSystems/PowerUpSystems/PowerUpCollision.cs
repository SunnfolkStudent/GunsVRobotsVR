using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCollision : MonoBehaviour
{
    public void AmIPickedUp(bool isPoweredUp)
    {
        if (!isPoweredUp)
        {
            Destroy(gameObject);
        }
    }
}
