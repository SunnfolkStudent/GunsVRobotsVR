using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GunSFXnVFXManager : MonoBehaviour
{
    public List<AudioClip> SFXList;
    public int currentWeapon;
    private List<VisualEffect> VFXlist;

    private void Start()
    {
        // Needs to add the source for accurate sound representation
        AudioManager.instance.TryAddSource(AudioManager.SoundType.Sfx, AudioManager.Source.Gun, gameObject);
    }
    
    
    public void onShoot()
    {
        AudioManager.instance.PlaySound(AudioManager.SoundType.Sfx, AudioManager.Source.Gun, SFXList[currentWeapon]); 
        
        VFXlist[currentWeapon].Play();
        
        VFXlist[currentWeapon].Reinit();
        VFXlist[currentWeapon].Stop();
    }
}
