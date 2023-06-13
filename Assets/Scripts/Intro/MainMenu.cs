using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MainMenu : MonoBehaviour
{
    //Note to self, learn how lists work
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject menuHands;
    [SerializeField] private GameObject gameHands;
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject soundSetting;
    [SerializeField] private GameObject controlsChart;
    [SerializeField] private GameObject fullUI;
    [SerializeField] private AudioClip[] introVoiceLines;
    private bool hasPlayedVoiceLine = false;

    private AudioSource _source;
    private void Start()
    {
        ToggleMenuMode(menuMode:true);
        _source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!hasPlayedVoiceLine && fullUI.transform.rotation.eulerAngles.x is < 275 and > 260)
        {
            hasPlayedVoiceLine = true;
            var index = UnityEngine.Random.Range(0, introVoiceLines.Length);
            //AudioManager.instance.TryPlaySound(AudioManager.SoundType.Voice, AudioManager.Source.Player, introVoiceLines[index]);
            _source.PlayOneShot(introVoiceLines[index]);
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