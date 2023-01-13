using UnityEngine;

[CreateAssetMenu (fileName = "EnemyStats", menuName = "Enemy/IntegrityStats")]

public class EnemyHealthScrub : ScriptableObject
{
    public float Integrity;
    public float Armour;
    public float Shield; 
}
