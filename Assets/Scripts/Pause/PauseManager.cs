using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static bool IsPaused;

    private XRIDefaultInputActions _inputs;

    private void Awake()
    {
        _inputs = new XRIDefaultInputActions();
    }

    private void OnEnable()
    {
        _inputs.Enable();
    }

    private void OnDisable()
    {
        _inputs.Disable();
    }

    public void Update()
    {
        if (_inputs.XRILeftHandInteraction.Pause.triggered)
        {
            IsPaused = !IsPaused;
            if (IsPaused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }
}
