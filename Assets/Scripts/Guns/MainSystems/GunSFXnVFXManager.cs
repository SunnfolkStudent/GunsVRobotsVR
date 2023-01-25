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

    private void Start()
    {
        // Needs to add the source for accurate sound representation
        AudioManager.instance.TryAddSource(AudioManager.SoundType.Sfx, AudioManager.Source.Gun, gameObject);
        //beamWeaponScript = GetComponentInChildren<BeamWeaponScript>();
        
        foreach (var VARIABLE in VFXlist)
        {
            VARIABLE.Reinit();
            
        }
    }

    private void OnDestroy()
    {
        AudioManager.instance.TryRemoveSource(AudioManager.SoundType.Sfx, AudioManager.Source.Gun, gameObject);
    }

    public void BeamVFXSFXInit()
    {
        AudioManager.instance.SetLooping(AudioManager.SoundType.Sfx, AudioManager.Source.Gun, true);
        AudioManager.instance.PlaySound(AudioManager.SoundType.Sfx, AudioManager.Source.Gun, SFXList[currentWeapon]); 
        VFXlist[currentWeapon].SetFloat("Distance", beamWeaponScript.distance);
        VFXlist[currentWeapon].Play();
    }

    public void BeamVFXSFXExit()
    {
        AudioManager.instance.SetLooping(AudioManager.SoundType.Sfx, AudioManager.Source.Gun, false);
        AudioManager.instance.StopAudio(AudioManager.SoundType.Sfx, AudioManager.Source.Gun);
        VFXlist[currentWeapon].Reinit();
        VFXlist[currentWeapon].Stop();
    }


    public void onShoot()
    {
        AudioManager.instance.PlaySound(AudioManager.SoundType.Sfx, AudioManager.Source.Gun, SFXList[currentWeapon]); 
        
        print("VFX should play");
        VFXlist[currentWeapon].Play();
        print("VFX played");
    }
}
