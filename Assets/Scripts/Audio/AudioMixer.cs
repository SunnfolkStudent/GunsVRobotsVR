using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public sealed class AudioMixer : MonoBehaviour
{
    public enum Source { Player, Enemy, Bullet }
    public static AudioMixer instance;

    [Range(0, 100)]
    public int sfxVolume;
    [Range(0, 100)]
    public int voiceVolume;
    [Range(0, 100)]
    public int musicVolume;

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
        if (sfxVolume != sfx.First().Value.volume)
        {
            foreach (var s in sfx)
            {
                s.Value.volume = (float)sfxVolume / 100;
            }
        }
        if (voiceVolume != voiceLines.First().Value.volume)
        {
            foreach (var s in voiceLines)
            {
                s.Value.volume = (float)voiceVolume / 100;
            }
        }
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
