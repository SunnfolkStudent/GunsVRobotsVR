using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GunSFXnVFXManager : MonoBehaviour
{
    public List<AudioClip> SFXList;
    public List<VisualEffect> VFXlist;
    public int currentWeapon;
    public BeamWeaponScript beamWeaponScript; 
    public AudioClip reloadClip; 

    private void Start()
    {
        // Needs to add the source for accurate sound representation
        AudioManager.instance.TryAddSource(AudioManager.SoundType.Sfx, AudioManager.Source.Gun, gameObject);
        beamWeaponScript = GetComponentInChildren<BeamWeaponScript>();
        
        foreach (var VARIABLE in VFXlist)
        {
            VARIABLE.Reinit();
        }
    }

    public void BeamVFXSFXInit()
    {
        AudioManager.instance.PlaySound(AudioManager.SoundType.Sfx, AudioManager.Source.Gun, SFXList[currentWeapon]); 
        
        print("BeamVFX");
        
        
        VFXlist[currentWeapon].SetFloat("Distance", beamWeaponScript.distance);
        VFXlist[currentWeapon].SendEvent("OnPlay");
    }

    public void BeamVFXSFCExit()
    {
        VFXlist[currentWeapon].Reinit();
        VFXlist[currentWeapon].SendEvent("OnStop");
    }


    public void onShoot()
    {
        AudioManager.instance.PlaySound(AudioManager.SoundType.Sfx, AudioManager.Source.Gun, SFXList[currentWeapon]);
        
        
        VFXlist[currentWeapon].Play();
    }

    public void OnReload()
    {
        AudioManager.instance.PlaySound(AudioManager.SoundType.Sfx, AudioManager.Source.Gun, reloadClip);
    }
}
