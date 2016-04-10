using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public void PressLocalGame()
    {
        SceneManager.LoadScene("Main");
    }
       
    private void Update()
    {
        GetComponent<Text>().text = "Player " + GameManager.win.ToString() + " Wins!";
    }
}
