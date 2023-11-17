using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public sealed class AudioManager : MonoBehaviour
{
    public enum Source { Player, Enemy, Bullet, Gun }
    public enum SoundType { Sfx, Voice }

    public static AudioManager instance;
    [HideInInspector]
    public FMODMusicManager fmodManager;
    public FMODUnity.EventReference musicPath;

    [Header("Volume control")]
    [Range(0, 1)]
    [SerializeField]
    private float masterVolume;
    [Range(0, 1)]
    [SerializeField]
    private float sfxVolume;
    [Range(0, 1)]
    [SerializeField]
    private float voiceVolume;
    [Range(0, 1)]
    [SerializeField]
    private float musicVolume;

    [HideInInspector]
    private Dictionary<Source, List<AudioSource>> sfx = new Dictionary<Source, List<AudioSource>>();
    [HideInInspector]
    private Dictionary<Source, List<AudioSource>> voiceLines = new Dictionary<Source, List<AudioSource>>();

    private AudioSource soundControlEcho;
    [SerializeField]
    private AudioClip soundControlClip;

    [SerializeField] private AudioSource sfxAudioSource, voiceAudioSource;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            
            instance = this;

            masterVolume = 1;
            sfxVolume = 1;
            voiceVolume = 1;
            musicVolume = 1;

            fmodManager = gameObject.AddComponent<FMODMusicManager>();
            fmodManager.ResetMusic();
            fmodManager.Init(musicPath);
            
            
            soundControlEcho = gameObject.AddComponent<AudioSource>();
            soundControlEcho.playOnAwake = false;
            soundControlEcho.clip = soundControlClip;

            sfx = new Dictionary<Source, List<AudioSource>>();
            voiceLines = new Dictionary<Source, List<AudioSource>>();

            DontDestroyOnLoad(gameObject);
        }
    }
    
    private void SoundTest(float volume)
    {
        soundControlEcho.volume = (float)volume;
        soundControlEcho.PlayDelayed(0.25f);
    }

    float CalculateVolume(float localVolume)
    {
        return (float)localVolume * (float)masterVolume;
    }
    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;
        UpdateSfxVolume();
        SoundTest(sfxVolume * masterVolume);
    }
    public void SetVoiceVolume(float volume)
    {
        voiceVolume = volume;
        UpdateVoiceVolume();
        SoundTest(voiceVolume * masterVolume);
    }
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        UpdateMusicVolume();
        SoundTest(musicVolume * masterVolume);
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        UpdateSfxVolume();
        UpdateVoiceVolume();
        UpdateMusicVolume();
        SoundTest(masterVolume);
    }
    void UpdateVolume(AudioSource source, float localVolume)
    {
        source.volume = CalculateVolume(localVolume);
    }
    void UpdateSfxVolume()
    {
        UpdateVolume(sfxAudioSource, sfxVolume);
    }
    void UpdateVoiceVolume()
    {
        UpdateVolume(voiceAudioSource, voiceVolume);
    }

    void UpdateMusicVolume()
    {
        if (fmodManager != null)
            fmodManager.SetVolume((float)musicVolume * (float)masterVolume);
    }
    void SetAudioSourceFields(AudioSource source, float volume)
    {
        source.volume = volume;
        source.playOnAwake = false;
        source.spatialBlend = 1; ;
    }

    private void SetLooping(Dictionary<Source, List<AudioSource>> dict, Source source, bool loop)
    {
        if (dict.TryGetValue(source, out List<AudioSource> list))
        {
            if (list.Count > 0)
            {
                list.First().loop = loop;
            }
        }
    }

    public void SetLooping(SoundType type, Source source, bool loop)
    {
        switch (type)
        {
            case SoundType.Sfx:
                SetLooping(sfx, source, loop);
                break;
            case SoundType.Voice:
                SetLooping(voiceLines, source, loop);
                break;
        }
    }

    private void StopAudio(AudioSource audioSource, Source source)
    {
        audioSource.Stop();
    }

    public void StopAudio(SoundType type, Source source)
    {
        switch (type)
        {
            case SoundType.Sfx:
                StopAudio(sfxAudioSource, source);
                break;
            case SoundType.Voice:
                StopAudio(voiceAudioSource, source);
                break;
        }
    }
    
    private bool TryAddSource(Dictionary<Source, List<AudioSource>> dict, Source source, GameObject gameObject)
    {
        if (dict.TryAdd(source, new List<AudioSource>()))
        {
            dict.Last().Value.Add(gameObject.AddComponent<AudioSource>());
            SetAudioSourceFields(dict.Last().Value.Last(), CalculateVolume(sfxVolume));
            return true;
        }
        else if (dict.TryGetValue(source, out List<AudioSource> list))
        {
            list.Add(gameObject.AddComponent<AudioSource>());
            SetAudioSourceFields(list.Last(), CalculateVolume(voiceVolume));
            return true;
        }
        return false;
    }
    public bool TryAddSource(SoundType type, Source source, GameObject gameObject)
    {
        return false;
        switch (type)
        {
            case SoundType.Sfx:
                TryAddSource(sfx, source, gameObject);
                return true;
            case SoundType.Voice:
                TryAddSource(voiceLines, source, gameObject);
                return true;
        }
        return false;
    }
    private bool TryClear(Dictionary<Source, List<AudioSource>> dict, Source source)
    {
        if (dict.TryGetValue(source, out List<AudioSource> list))
        {
            list.Clear();
            return true;
        }
        return false;
    }
    public bool TryClear(SoundType type, Source source)
    {
        return false;
        switch (type)
        {
            case SoundType.Sfx:
                TryClear(sfx, source);
                return true;
            case SoundType.Voice:
                TryClear(voiceLines, source);
                return true;
        }
        return false;
    }
    private bool TryRemoveSource(Dictionary<Source, List<AudioSource>> dict, Source source, int index)
    {
        if (dict.TryGetValue(source, out List<AudioSource> list))
        {
            if (list != null && list.Count > index) list.RemoveAt(index);
            return true;
        }
        return false;
    }
    private bool TryRemoveSource(Dictionary<Source, List<AudioSource>> dict, Source source, GameObject gameObject)
    {
        if (dict.TryGetValue(source, out List<AudioSource> list))
        {
            if (list != null && list.Contains(gameObject.GetComponent<AudioSource>())) list.Remove(gameObject.GetComponent<AudioSource>());
            return true;
        }
        return false;
    }
    public bool TryRemoveSource(SoundType type, Source source, int index)
    {
        return false;
        switch (type)
        {
            case SoundType.Sfx:
                TryRemoveSource(sfx, source, index);
                return true;
            case SoundType.Voice:
                TryRemoveSource(voiceLines, source, index);
                return true;
        }
        return false;
    }

    public bool TryRemoveSource(SoundType type, Source source, GameObject gameObject)
    {
        return false;
        switch (type)
        {
            case SoundType.Sfx:
                TryRemoveSource(sfx, source, gameObject);
                return true;
            case SoundType.Voice:
                TryRemoveSource(voiceLines, source, gameObject);
                return true;
        }
        return false;
    }

    // Not in use and not up to date anymore
    /*private bool TryGetSource(Dictionary<Source, List<AudioSource>> dict, Source source, out AudioSource s)
    {
        if (dict.TryGetValue(source, out AudioSource audioSource))
        {
            if (audioSource != null)
            {
                s = audioSource;
                return true;
            }
        }
        s = null;
        return false;
    }
    public bool TryGetSource(SoundType type, Source source, out AudioSource s)
    {
        switch (type)
        {
            case SoundType.Sfx:
                TryGetSource(sfx, source, out s);
                return true;
                break;
            case SoundType.Voice:
                TryGetSource(voiceLines, source, out s);
                return true;
                break;
        }
        s = null;
        return false;
    }*/

    private void PlaySound(AudioSource audioSource, Source source, int index, AudioClip clip, bool canAlwaysPlay)
    {
        if (!clip) return;
        audioSource.PlayOneShot(clip);
    }
    public void PlaySound(GameObject gameObject, AudioClip clip, bool canAlwaysPlay = true)
    {
        if (!clip) return;
        
        sfxAudioSource.PlayOneShot(clip);
        return;
        
        AudioSource s = gameObject.GetComponent<AudioSource>();
       // s.volume = 1;
       // s.minDistance = 100;
       // s.maxDistance = 500;
       
        if (!canAlwaysPlay && s.isPlaying)
            return;
        //TODO I added this fix for audio
        if (s == null || clip == null) return;
            s.PlayOneShot(clip);
    }
    private void PlaySound(SoundType type, Source source, AudioClip clip, int sourceIndex, bool canAlwaysPlay)
    {
        switch (type)
        {
            case SoundType.Sfx:
                PlaySound(sfxAudioSource, source, sourceIndex, clip, canAlwaysPlay);
                break;
            case SoundType.Voice:
                PlaySound(voiceAudioSource, source, sourceIndex, clip, canAlwaysPlay);
                break;
        }
    }
    public void TryPlaySound(SoundType type, Source source, AudioClip clip, int sourceIndex)
    {
        PlaySound(type, source, clip, sourceIndex, false);
    }
    public void PlaySound(SoundType type, Source source, AudioClip clip, int sourceIndex)
    {
        PlaySound(type, source, clip, sourceIndex, true);
    }
    public void TryPlaySound(SoundType type, Source source, AudioClip clip)
    {
        PlaySound(type, source, clip, 0, false);
    }
    public void PlaySound(SoundType type, Source source, AudioClip clip)
    {
        PlaySound(type, source, clip, 0, true);
    }
}
