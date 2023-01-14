using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public sealed class AudioManager : MonoBehaviour
{
    public enum Source { Player, Enemy, Bullet, Gun }
    public static AudioManager instance;
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
    private readonly int _maxVolume = 100;

    [HideInInspector]
    public Dictionary<Source, Sound> sfx;
    [HideInInspector]
    public Dictionary<Source, Sound> voiceLines;

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

            sfx = new Dictionary<Source, Sound>();
            voiceLines = new Dictionary<Source, Sound>();

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
                s.Value.SetVolume((float)sfxVolume * (float)masterVolume / 10000);
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
                s.Value.SetVolume((float)voiceVolume * (float)masterVolume / 10000);
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

    public bool TryAddSfxSource(Source source, GameObject gameObject)
    {
        return sfx.TryAdd(source, new Sound(gameObject)) ;
    }
    public bool TryAddVoiceSource(Source source, GameObject gameObject)
    {
        return voiceLines.TryAdd(source, new Sound(gameObject));
    }

    private bool TryGetSource(Dictionary<Source, Sound> dict, Source source, out AudioSource s)
    {
        if (dict.TryGetValue(source, out Sound sound))
        {
            if (sound.source != null)
            {
                s = sound.source;
                return true;
            }
        }
        s = null;
        return false;
    }
    public bool TryGetSfxSource(Source source, out AudioSource s)
    {
        return TryGetSource(sfx, source, out s);
    }
    public bool TryGetVoiceSource(Source source, out AudioSource s)
    {
        return TryGetSource(voiceLines, source, out s);
    }

    private bool TryAddEvent(Dictionary<Source, Sound> dict, Source source, string name, UnityAction action)
    {
        if (dict.TryGetValue(source, out Sound sound))
        {
            sound.TryAddEvent(name, action);
        }
        return false;
    }
    public bool TryAddSfxEvent(Source source, string name, UnityAction action)
    {
        return TryAddEvent(sfx, source, name, action);
    }
    public bool TryAddVoiceEvent(Source source, string name, UnityAction action)
    {
        return TryAddEvent(voiceLines, source, name, action);
    }

    private bool TryGetEvent(Dictionary<Source, Sound> dict, Source source, string name, out UnityEvent e)
    {
        if (dict.TryGetValue(source, out Sound sound))
        {
            return sound.TryGetEvent(name, out e);
        }
        e = null;
        return false;
    }
    public bool TryGetSfxEvent(Source source, string name, out UnityEvent e)
    {
        return TryGetEvent(sfx, source, name, out e);
    }
    public bool TryGetVoiceEvent(Source source, string name, out UnityEvent e)
    {
        return TryGetEvent(voiceLines, source, name, out e);
    }
}
