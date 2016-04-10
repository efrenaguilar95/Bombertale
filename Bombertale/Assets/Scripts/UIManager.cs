using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public void PressLocalGame()
    {
        SceneManager.LoadScene("Main");
    }
       
    private void Start()
    {
        GameObject Text = GameObject.Find("Winner");
        if (Text != null)
            {
            GetComponent<Text>().text = "Player " + GameManager.win.ToString() + " Wins!";
        }
    }
}
