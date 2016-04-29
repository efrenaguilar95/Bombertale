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

    public override void Awake()
    {
        base.Awake();
        int randomPort = Random.Range(10000, 65000);
        base.Setup(randomPort, 1);
        _server = base.Connect(NetworkHost.ServerIP, NetworkHost.Port);

        _playersInLobby = GameObject.Find("PlayersInLobbyText").GetComponent<Text>();
    }

    void Update()
    {
        int connectionID;
        int channelID;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;
        NetworkEventType recData = NetworkTransport.ReceiveFromHost(this._hostID, out connectionID, out channelID, recBuffer, bufferSize, out dataSize, out error);
        if (recData == NetworkEventType.DataEvent)
        {
            _playerCount = int.Parse(System.Text.Encoding.UTF8.GetString(recBuffer));
        }
    }

    void LateUpdate()
    {
        _playersInLobby.text = "Players: " + this._playerCount + "/4";
    }
}
