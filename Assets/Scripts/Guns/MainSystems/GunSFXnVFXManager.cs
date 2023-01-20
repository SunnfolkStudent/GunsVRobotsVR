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
    private BeamWeaponScript beamWeaponScript; 

    private void Start()
    {
        // Needs to add the source for accurate sound representation
        AudioManager.instance.TryAddSource(AudioManager.SoundType.Sfx, AudioManager.Source.Gun, gameObject);
        beamWeaponScript = GetComponentInParent<BeamWeaponScript>(); 
    }

    public void BeamVFXSFXInit()
    {
        AudioManager.instance.PlaySound(AudioManager.SoundType.Sfx, AudioManager.Source.Gun, SFXList[currentWeapon]); 
        
        VFXlist[currentWeapon].Play();
        
        VFXlist[currentWeapon].SetFloat("Distance", beamWeaponScript.distance);
    }

    public void BeamVFXSFCExit()
    {
        VFXlist[currentWeapon].Reinit();
        VFXlist[currentWeapon].Stop();
    }


    public void onShoot()
    {
        /*AudioManager.instance.PlaySound(AudioManager.SoundType.Sfx, AudioManager.Source.Gun, SFXList[currentWeapon]);*/ 
        
        print("VFX should play");
        VFXlist[currentWeapon].Play();
        print("VFX played"); 
        VFXlist[currentWeapon].Reinit();
        VFXlist[currentWeapon].Stop();
    }
}
