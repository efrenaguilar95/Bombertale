using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WinnerDisplay : MonoBehaviour
{

    void Start()
    {        
        this.GetComponent<Text>().text = "Player " + GameManager.win.ToString() + " Wins!";        
    }
}