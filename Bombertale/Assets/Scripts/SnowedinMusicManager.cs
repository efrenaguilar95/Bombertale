using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SnowedinMusicManager : MonoBehaviour {
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void OnLevelWasLoaded(int level)
    {        
        string loadedScene = SceneManager.GetActiveScene().name;
        if (loadedScene == "MainMenu" || loadedScene == "ServerGame" || loadedScene == "ClientGame")
        {
            Destroy(this.gameObject);
        }
    }
}
