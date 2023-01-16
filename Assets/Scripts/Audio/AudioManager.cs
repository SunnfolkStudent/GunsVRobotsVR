using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public sealed class AudioManager : MonoBehaviour
{
    public enum Source { Player, Enemy, Bullet, Gun }
    public enum SoundType {  Sfx, Voice }

    public static AudioManager instance;
    private FMODMusicManager fmodManager;

    [Header("Volume control")]
    [Range(0, 100)]
    public int masterVolume;
    private int prevMasterVolume;
    [Range(0, 100)]
    public int sfxVolume;
    private int prevSfxVolume;
    [Range(0, 100)]
    public int voiceVolume;
    private int prevVoiceVolume;
    [Range(0, 100)]
    public int musicVolume;
    private int prevMusicVolume;

    // A shared max volume for all channels
    private readonly int _maxVolume = 100;

    [HideInInspector]
    public Dictionary<Source, List<AudioSource>> sfx;
    [HideInInspector]
    public Dictionary<Source, List<AudioSource>> voiceLines;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;

            /*masterVolume = _maxVolume;
            sfxVolume = _maxVolume;
            voiceVolume = _maxVolume;
            musicVolume = _maxVolume;*/

            sfx = new Dictionary<Source, List<AudioSource>>();
            voiceLines = new Dictionary<Source, List<AudioSource>>();

            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Update()
    {
        if (prevMasterVolume != masterVolume)
        {
            prevMasterVolume = masterVolume;
            UpdateSfxVolume();
            UpdateVoiceVolume();
            UpdateMusicVolume();
        }
        if (prevSfxVolume != sfxVolume)
        {
            prevSfxVolume = sfxVolume;
            UpdateSfxVolume();
        }
        if (prevVoiceVolume != voiceVolume)
        {
            prevVoiceVolume = voiceVolume;
            UpdateVoiceVolume();
        }
        if (prevMusicVolume != musicVolume)
        {
            prevMusicVolume = musicVolume;
            UpdateMusicVolume();
        }
    }

    float CalculateVolume(int localVolume)
    {
        return (float)localVolume * (float)masterVolume / 10000;
    }
    void UpdateVolume(Dictionary<Source, List<AudioSource>> dict, int localVolume)
    {
        foreach (var list in dict)
        {
            // Divide by 100 * 100 == 10000
            foreach (var s in list.Value)
            {
                s.volume = CalculateVolume(localVolume);
            }
        }
    }
    void UpdateSfxVolume()
    {
        UpdateVolume(sfx, sfxVolume);
    }
    void UpdateVoiceVolume()
    {
        UpdateVolume(voiceLines, voiceVolume);
    }

    void UpdateMusicVolume()
    {
        if (fmodManager != null)
            fmodManager.SetVolume((float)musicVolume * (float)masterVolume / 10000);
    }

    public void SetFmodManager(FMODMusicManager fmod)
    {
        fmodManager = fmod;
    }
    void SetAudioSourceFields(AudioSource source, float volume)
    {
        source.volume = volume;
        source.playOnAwake = false;
        source.spatialBlend = 1; ;
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

    private bool TryRemoveSource(Dictionary<Source, List<AudioSource>> dict, Source source, int index)
    {
        if (dict.TryGetValue(source, out List<AudioSource> list))
        {
            list.RemoveAt(index);
            return true;
        }
        return false;
    }
    private bool TryRemoveSource(Dictionary<Source, List<AudioSource>> dict, Source source, GameObject gameObject)
    {
        if (dict.TryGetValue(source, out List<AudioSource> list))
        {
            list.Remove(gameObject.GetComponent<AudioSource>());
            return true;
        }
        return false;
    }
    public bool TryRemoveSource(SoundType type, Source source, int index)
    {
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

    private void PlaySound(Dictionary<Source, List<AudioSource>> dict, Source source, int index, AudioClip clip, bool canAlwaysPlay)
    {
        if (dict.TryGetValue(source, out List<AudioSource> list))
        {
            if (!canAlwaysPlay && list[index].isPlaying)
                return;
            list[index].PlayOneShot(clip);
        }
    }
    private void PlaySound(SoundType type, Source source, AudioClip clip, int sourceIndex, bool canAlwaysPlay)
    {
        switch (type)
        {
            case SoundType.Sfx:
                PlaySound(sfx, source, sourceIndex, clip, canAlwaysPlay);
                break;
            case SoundType.Voice:
                PlaySound(voiceLines, source, sourceIndex, clip, canAlwaysPlay);
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
