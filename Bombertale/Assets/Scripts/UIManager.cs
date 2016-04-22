using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public void PressLocalGame()
    {
        SceneManager.LoadScene("Main");
    }
    public void PressJoinLobby()
    {
        SceneManager.LoadScene("Lobby");
    } 
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
