using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class FMODMusicManager : MonoBehaviour
{
    public EventReference path;

    // Player Health
    [Range(0, 100)]
    public int health;

    // Remove this if we can call an event once and have no need to care for it
    public bool isFirstEnemyShot = false;
    public bool isInGame = false;

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

        // Would like for this to be called through an event
        SetFmodGlobalParam("menu", System.Convert.ToSingle(isFirstEnemyShot));
        SetFmodGlobalParam("game", System.Convert.ToSingle(isInGame));
    }
    public void SetVolume(float volume)
    {
        instance.setVolume(volume);
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
