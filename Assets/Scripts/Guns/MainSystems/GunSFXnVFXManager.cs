using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSFXnVFXManager : MonoBehaviour
{
    public List<AudioClip> SFXList;
    public int currentWeapon;

    public void onShoot()
    {
        AudioManager.instance.PlaySound(AudioManager.SoundType.Sfx, AudioManager.Source.Gun, SFXList[currentWeapon]); 
    }
}
