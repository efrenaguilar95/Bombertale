using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LocalGameWinnerDisplay : MonoBehaviour
{

    void Start()
    {        
        this.GetComponent<Text>().text = "Player " + LocalGameManager.win.ToString() + " Wins!";        
    }
}