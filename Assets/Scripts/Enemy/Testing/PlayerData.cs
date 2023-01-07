using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerPos")]
public class PlayerData : ScriptableObject
{
    public Vector3 position;
}
