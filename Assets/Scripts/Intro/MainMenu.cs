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
    [SerializeField] private GameObject soundSetting;
    [SerializeField] private GameObject controlsChart;
    [SerializeField] private GameObject fullUI;
    [SerializeField] private AudioClip[] introVoiceLines;
    private bool hasPlayedVoiceLine = false;

    private void Start()
    {
        ToggleMenuMode(menuMode:true);
        RightHandSelect();
    }

    void Update()
    {
        if (!hasPlayedVoiceLine && fullUI.transform.rotation.eulerAngles.x is < 275 and > 260)
        {
            hasPlayedVoiceLine = true;
            var index = UnityEngine.Random.Range(0, introVoiceLines.Length);
            AudioManager.instance.TryPlaySound(AudioManager.SoundType.Voice, AudioManager.Source.Player, introVoiceLines[index]);
        }
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
        soundSetting.SetActive(true);
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

    public void Sounds()
    {
        soundSetting.SetActive(false);
        controlsChart.SetActive(true);
    }
    public void ControlsChartNext()
    {
        rb.useGravity = true;
        ToggleMenuMode(menuMode:false);
    }
}