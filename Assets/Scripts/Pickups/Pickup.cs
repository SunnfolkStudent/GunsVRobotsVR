using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    public float pickUpTime;
    public float pickUpRadius = 8f;
    public float amountToHeal;
    public float ammoToRefill;
    private bool _isBeingPickedUp;

    private void Update()
    {
        if (_isBeingPickedUp) return;
        if ((transform.position - _playerData.position).magnitude > pickUpRadius) return;
        
        var playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (var playerObject in playerObjects)
        {
            if (playerObject.TryGetComponent<PlayerHealthManager>(out var player))
            {
                StartCoroutine(InterpolateTowardsPlayer(transform.position, player));
                break;
            }
        }
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
