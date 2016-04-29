using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Remove after inheritance
using UnityEngine.Networking;

public class ClientManager : NetworkHost
{
    private int _server;
    private int _playerCount;

    /// Lobby Variables
    private Text _playersInLobby;

    void Awake()
    {        
        int randomPort = Random.Range(10000, 65000);
        base.Setup(randomPort, 1);
        _server = base.Connect(NetworkHost.ServerIP, NetworkHost.Port);

        _playersInLobby = GameObject.Find("PlayersInLobbyText").GetComponent<Text>();
    }

    void Update()
    {
        ReceiveEvent recEvent = base.Receive();
        if (recEvent.type == NetworkEventType.DataEvent)
        {                   
            Message message = recEvent.message;
            Debug.Log(message.subJson);
            if (message.type == MessageType.LobbyUpdate)
            {
                LobbyUpdate lobbyUpdate = (LobbyUpdate)message.GetData();
                _playerCount = lobbyUpdate.playerCount;
            }
        }
    }

    void LateUpdate()
    {
        _playersInLobby.text = "Players: " + this._playerCount + "/4";
    }
}
