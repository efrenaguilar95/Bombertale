using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public void PressLocalGame()
    {
        SceneManager.LoadScene("Main");
    }      
}
