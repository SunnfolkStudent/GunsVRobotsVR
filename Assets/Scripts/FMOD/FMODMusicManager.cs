using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class FMODMusicManager : MonoBehaviour
{
    public EventReference path;

    [Range(0, 1)]
    public float volume;
    // Player Health
    [Range(0, 100)]
    public int health;
    [Range(0, 10)]
    public int wave;

    public bool isFirstEnemyShot = false;

    private FMOD.Studio.EventInstance instance;

    private FMOD.Studio.PARAMETER_DESCRIPTION healthParam;
    private FMOD.Studio.PARAMETER_DESCRIPTION waveParam;

    // Start is called before the first frame update
    void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(path);
        instance.start();

        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, GetComponent<Transform>(),
            GetComponent<Rigidbody>());
        GetFmodParamDescription("song type", out healthParam);
        GetFmodParamDescription("wave", out waveParam);
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 75)
        {
            SetFmodLocalParam(healthParam.id, 0);
        }
        else if (health is > 50 and < 75)
        {
            SetFmodLocalParam(healthParam.id, 1);
        }
        else if (health is > 25 and < 50)
        {
            SetFmodLocalParam(healthParam.id, 2);
        }
        else if (health < 25)
        {
            SetFmodLocalParam(healthParam.id, 3);
        }

        instance.setVolume(volume);

        SetFmodGlobalParam("wave", wave);
        SetFmodGlobalParam("first kill name", System.Convert.ToSingle(isFirstEnemyShot));
    }

    void SetFmodLocalParam(FMOD.Studio.PARAMETER_ID id, float value)
    {
        RuntimeManager.StudioSystem.setParameterByID(id, value);
    }
    void SetFmodGlobalParam(string name, float value)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(name, value);
    }

    void GetFmodParamDescription(string name, out FMOD.Studio.PARAMETER_DESCRIPTION param)
    {
        FMODUnity.RuntimeManager.StudioSystem.getParameterDescriptionByName(name, out param);
    }
}
