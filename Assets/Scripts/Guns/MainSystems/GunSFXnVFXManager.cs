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
        AudioManager.instance.PlaySound(AudioManager.soundType.sfx, AudioManager.Source.Gun, SFXList[currentWeapon]); 
    }
}
