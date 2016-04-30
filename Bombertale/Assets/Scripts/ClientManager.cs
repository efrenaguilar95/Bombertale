using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

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
        PollMovement();
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
            if (message.type == MessageType.Setup)
            {
                SceneManager.LoadScene("ClientGame");
            }
        }
    }

    void LateUpdate()
    {
        _playersInLobby.text = "Players: " + this._playerCount + "/4";
    }

    private void PollMovement()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            //Serialize a keypress
            //Send this
            base.Send(_server, MessageType.Move, new Move (Direction.UP));
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            base.Send(_server, MessageType.Move, new Move (Direction.LEFT));
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            base.Send(_server, MessageType.Move,new Move (Direction.DOWN));
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            base.Send(_server, MessageType.Move, new Move (Direction.RIGHT));
        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            base.Send(_server, MessageType.Move, new Move(Direction.NONE));
        }
    }
}
