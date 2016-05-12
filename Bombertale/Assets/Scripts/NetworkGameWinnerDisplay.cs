using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkGameWinnerDisplay : MonoBehaviour
{

    private ClientManager _clientManager;

    void Awake()
    {
        _clientManager = GameObject.Find("NetworkManager").GetComponent<ClientManager>();
    }

    void Start()
    {        
        this.GetComponent<Text>().text = _clientManager.winnerMessage;        
    }
}