using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    
    public void MainMenu()
    {
        //AudioManager.instance.fmodManager.Init(AudioManager.instance.musicPath);
        //AudioManager.instance.fmodManager.ResetMusic();
        SceneManager.LoadScene("IntroScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
