using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   //DO NOT PUT THIS IN INTRO SCENE OR END SCREEN
    
    private FadeScript _fade;
    private DialogLineManager _lineManager;
    private IntroScene _intro;
    private Scene _scene;

    [SerializeField] private Vector3 _nextLevelTriggerSpawnPosition;
    [SerializeField] private GameObject _nextLevelTriggerPrefab;

     private void Awake()
    {
        _fade = GetComponent<FadeScript>();
        
        Debug.Log("Active Scene name is: " + _scene.name + "\nActive Scene index: " + _scene.buildIndex);
    }

     private void Start()
     {
         _fade.HideUi();
         AudioManager.instance.fmodManager.SetGameState(FMODMusicManager.GameState.Game);
         switch (SceneManager.GetActiveScene().name)
         {
             case "Arena_1":
             case "Arena_1_Test":
                 AudioManager.instance.fmodManager.SetArena(0);
                 break;
             case "Arena_2":
             case "Arena_2_Test":
                 AudioManager.instance.fmodManager.SetArena(1);
                 break;
             case "Boss":
             case "Boss_Test":
                 AudioManager.instance.fmodManager.SetArena(2);
                 break;
         }
     }

    public void SpawnNextLevelTrigger()
    {
        Instantiate(_nextLevelTriggerPrefab, _nextLevelTriggerSpawnPosition, Quaternion.identity);
    }
    
    public void OnNextLevelInteract()
    {
        //when fade alpha is at 1 go to next level
        _fade.ShowUi();
    }
}