using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class FMODMusicManager : MonoBehaviour
{
    public enum GameState : int { Menu, Game, Credits }
    public enum ArenaState : int { Start, Fighting, Finished }
    public EventReference path;

    private FMOD.Studio.EventInstance instance;

    private FMOD.Studio.PARAMETER_DESCRIPTION healthParam;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.SetFmodManager(this);

        instance = FMODUnity.RuntimeManager.CreateInstance(path);
        instance.start();

        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, GetComponent<Transform>(),
            GetComponent<Rigidbody>());
        GetFmodParamDescription("health", out healthParam);
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void SetVolume(float volume)
    {
        instance.setVolume(volume);
    }
    public void ResetMusic()
    {
        SetFmodGlobalParam("menu", 0);
        SetFmodGlobalParam("game", 0);
    }
    public void SetGameMusicState(GameState state)
    {
        SetFmodGlobalParam("game", (float)state);
        if (state == GameState.Menu)
            SetFmodGlobalParam("menu", 1);
    }
    public void SetArenaMusic(int arena)
    {
        SetFmodGlobalParam("arena", (float)arena);
    }
    public void SetArenaState(ArenaState state)
    {
        SetFmodLocalParam("arena region", (float)state);
    }
    public void SetHealth(int health)
    {
        if (health > 80)
        {
            SetFmodGlobalParam(healthParam.id, 4);
        }
        else if (health is > 60 and < 80)
        {
            SetFmodGlobalParam(healthParam.id, 3);
        }
        else if (health is > 40 and < 60)
        {
            SetFmodGlobalParam(healthParam.id, 2);
        }
        else if (health is > 20 and < 40)
        {
            SetFmodGlobalParam(healthParam.id, 1);
        }
        else
        {
            SetFmodGlobalParam(healthParam.id, 0);
        }
    }

    #region FMODHelpers
    void SetFmodLocalParam(FMOD.Studio.PARAMETER_ID id, float value)
    {
        RuntimeManager.StudioSystem.setParameterByID(id, value);
    }
    void SetFmodLocalParam(string name, float value)
    {
        RuntimeManager.StudioSystem.setParameterByName(name, value);
    }
    void SetFmodGlobalParam(FMOD.Studio.PARAMETER_ID id, float value)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByID(id, value);
    }
    void SetFmodGlobalParam(string name, float value)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(name, value);
    }

    void GetFmodParamDescription(string name, out FMOD.Studio.PARAMETER_DESCRIPTION param)
    {
        FMODUnity.RuntimeManager.StudioSystem.getParameterDescriptionByName(name, out param);
    }
    #endregion
}
