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
     }

    public void SpawnNextLevelTrigger()
    {
        if (EnemyPoolController.CurrentEnemyPoolController.activeEnemies.Count == 0 )
        {
            Instantiate(_nextLevelTriggerPrefab, _nextLevelTriggerSpawnPosition, Quaternion.identity);
        }
    }
    
    public void OnNextLevelInteract()
    {
        //when fade alpha is at 1 go to next level
        _fade.ShowUi();
    }
}