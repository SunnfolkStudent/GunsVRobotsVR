using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IntroHandler : MonoBehaviour
{
    [SerializeField]
    private LerpPos gameLogo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            gameLogo.Spawn();
            AudioManager.instance.fmodManager.SetGameMusicState(FMODMusicManager.GameState.Menu);
        }

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            gameLogo.ResetPos();
            AudioManager.instance.fmodManager.ResetMusic();
        }
    }
}
