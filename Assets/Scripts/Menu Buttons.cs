using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    
    public void MainMenu()
    {
        /*SceneManager.SetActiveScene("IntroScene");*/
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
