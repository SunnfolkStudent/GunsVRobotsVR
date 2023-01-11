using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private SphereCollider _trigger;
    [SerializeField] private PlayerData _playerData;
    public float pickUpTime;
    public float amountToHeal;
    public float ammoToRefill;

    public void MoveTowardsPlayer(PlayerHealthManager player)
    {
        var startPosition = transform.position;
        StartCoroutine(InterpolateTowardsPlayer(startPosition, player));
    }

    private IEnumerator InterpolateTowardsPlayer(Vector3 startPosition, PlayerHealthManager player)
    {
        var startTime = Time.time;
        while (Time.time < startTime + pickUpTime)
        {
            transform.position = Vector3.Lerp(startPosition, _playerData.position, (Time.time - startTime) / pickUpTime);
            yield return null;
        }

        if (amountToHeal > 0f)
        {
            player.HealDamage(amountToHeal);
        }

        if (ammoToRefill > 0f)
        {
            player.RefillAmmo(ammoToRefill);
        }

        Destroy(gameObject);
    }
}
