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
            int rand = UnityEngine.Random.Range(0, player._playerAudio.onPickupHealth.Length);
            AudioManager.instance.PlaySound(AudioManager.SoundType.Sfx, AudioManager.Source.Player, player._playerAudio.onPickupHealth[rand]);
        }

        if (ammoToRefill > 0f)
        {
            player.RefillAmmo(ammoToRefill);
            int rand = UnityEngine.Random.Range(0, player._playerAudio.onPickupAmmo.Length);
            AudioManager.instance.PlaySound(AudioManager.SoundType.Sfx, AudioManager.Source.Player, player._playerAudio.onPickupAmmo[rand]);
        }

        Destroy(gameObject);
    }
}
