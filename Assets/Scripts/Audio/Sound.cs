using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Sound
{
    [HideInInspector]
    public AudioSource source;
    private Dictionary<string, UnityEvent> events;

    public Sound(GameObject gameObject)
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        events = new Dictionary<string, UnityEvent>();
    }

    public void SetVolume(float volume)
    {
        source.volume = volume;
    }

    public bool TryAddEvent(string name, UnityAction action)
    {
        if (events.TryAdd(name, new UnityEvent()))
        {
            events.Last().Value.AddListener(action);
            return true;
        }
        else if (events.TryGetValue(name, out UnityEvent e))
        {
            e.AddListener(action);
            return true;
        }
        return false;
    }
    public bool TryGetEvent(string name, out UnityEvent e)
    {
        if (events.TryGetValue(name, out e))
        {
            return true;
        }
        e = null;
        return false;
    }
}
