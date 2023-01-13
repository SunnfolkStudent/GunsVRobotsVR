using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public sealed class AudioMixer : MonoBehaviour
{
    public enum Source { Player, Enemy, Bullet }
    public static AudioMixer instance;
    private FMODMusicManager fmodManager;

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
    private int _maxVolume = 100;

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

            masterVolume = _maxVolume;
            sfxVolume = _maxVolume;
            voiceVolume = _maxVolume;
            musicVolume = _maxVolume;

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
        if (sfx.Count > 0)
        {
            foreach (var s in sfx)
            {
                // Divide by 100 * 100 == 10000
                s.Value.volume = (float)sfxVolume * (float)masterVolume / 10000;
            }
        }
    }

    void UpdateVoiceVolume()
    {
        if (voiceLines.Count > 0)
        {
            foreach (var s in voiceLines)
            {
                // Divide by 100 * 100 == 10000
                s.Value.volume = (float)voiceVolume * (float)masterVolume / 10000;
            }
        }
    }
    void UpdateMusicVolume()
    {
        fmodManager.SetVolume((float)musicVolume * (float)masterVolume / 10000);
    }

    public void SetFmodManager(FMODMusicManager fmod)
    {
        fmodManager = fmod;
    }

    public void AddSfxSource(Source source, GameObject gameObject)
    {
        sfx.TryAdd(source, gameObject.AddComponent<AudioSource>());
        sfx[source].playOnAwake = false;
    }
    public void AddVoiceSource(Source source, GameObject gameObject)
    {
        voiceLines.TryAdd(source, gameObject.AddComponent<AudioSource>());
        voiceLines[source].playOnAwake = false;
    }
}
