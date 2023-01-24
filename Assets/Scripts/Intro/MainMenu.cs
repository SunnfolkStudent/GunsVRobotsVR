using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MainMenu : MonoBehaviour
{
    //Note to self, learn how lists work
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject selectedLeft, selectedRight;
    [SerializeField] private GameObject menuHands;
    [SerializeField] private GameObject gameHands;
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject handSetting;
    [SerializeField] private GameObject soundSetting;
    [SerializeField] private GameObject controlsChart;
    [SerializeField] private GameObject fullUI;
    [SerializeField] private AudioClip[] introVoiceLines;

    private void Start()
    {
        ToggleMenuMode(menuMode:true);
        RightHandSelect();
    }

    private void ToggleMenuMode(bool menuMode)
    {
        if (menuMode)
        {
            menuHands.SetActive(true);
            gameHands.SetActive(false);
        }
        else if (!menuMode)
        {
            menuHands.SetActive(false);
            gameHands.SetActive(true);
        }
    }

    public void StartButton()
    {
        main.SetActive(false);
        handSetting.SetActive(true);
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LeftHandSelect()
    {
        //Gun in left hand
        //Teleport with right hand
        HandController.teleportHand = false;
        selectedLeft.SetActive(true);
        selectedRight.SetActive(false);
    }

    public void RightHandSelect()
    {
        //Gun in right hand
        //Teleport with left hand
        HandController.teleportHand = true;
        selectedRight.SetActive(true);
        selectedLeft.SetActive(false);
    }

    public void HandNextButton()
    {
        handSetting.SetActive(false);
        soundSetting.SetActive(true);
    }

    public void Sounds()
    {
        soundSetting.SetActive(false);
        controlsChart.SetActive(true);
    }
    public void ControlsChartNext()
    {
        rb.useGravity = true;
        var index = UnityEngine.Random.Range(0, introVoiceLines.Length);
        AudioManager.instance.TryPlaySound(AudioManager.SoundType.Voice, AudioManager.Source.Player, introVoiceLines[index]);
        ToggleMenuMode(menuMode:false);
    }
}