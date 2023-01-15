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
    public Dictionary<Source, AudioSource> sfx;
    [HideInInspector]
    public Dictionary<Source, AudioSource> voiceLines;

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

            sfx = new Dictionary<Source, AudioSource>();
            voiceLines = new Dictionary<Source, AudioSource>();

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

    void UpdateSfxVolume()
    {
        foreach (var s in sfx)
        {
            // Divide by 100 * 100 == 10000
            s.Value.volume = ((float)sfxVolume * (float)masterVolume / 10000);
        }
    }

    void UpdateVoiceVolume()
    {
        foreach (var s in voiceLines)
        {
            // Divide by 100 * 100 == 10000
            s.Value.volume = ((float)voiceVolume * (float)masterVolume / 10000);
        }
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
    private bool TryAddSource(Dictionary<Source, AudioSource> dict, Source source, GameObject gameObject)
    {
        if (dict.TryAdd(source, gameObject.AddComponent<AudioSource>()))
        {
            dict.Last().Value.playOnAwake = false;
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
                break;
            case SoundType.Voice:
                TryAddSource(voiceLines, source, gameObject);
                return true;
                break;
        }
        return false;
    }
    private bool TryGetSource(Dictionary<Source, AudioSource> dict, Source source, out AudioSource s)
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
    }

    private bool PlaySound(Dictionary<Source, AudioSource> dict, Source source, AudioClip clip)
    {
        if (dict.TryGetValue(source, out AudioSource s))
        {
            s.PlayOneShot(clip);
            return true;
        }
        return false;
    }
    public bool PlaySound(SoundType type, Source source, AudioClip clip)
    {
        switch (type)
        {
            case SoundType.Sfx:
                PlaySound(sfx, source, clip);
                return true;
                break;
            case SoundType.Voice:
                PlaySound(voiceLines, source, clip);
                return true;
                break;
        }
        return false;
    }
}
