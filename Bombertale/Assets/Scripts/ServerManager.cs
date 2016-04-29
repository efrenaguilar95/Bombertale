using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//Take this out after inheritance
using UnityEngine.Networking;

public class ServerManager : NetworkHost {
    public List<int> clientList = new List<int>();

    private DatabaseManager _databaseManager;

    /// Lobby Variables
    //private Text _playersInLobby;

    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
        base.Setup(NetworkHost.Port, 4);
        _databaseManager = this.GetComponent<DatabaseManager>();
        _databaseManager.CreateServer("Bombertale", NetworkHost.ServerIP, NetworkHost.Port, false, "", clientList.Count);

        //_playersInLobby = GameObject.Find("PlayersInLobbyText").GetComponent<Text>();
    }

    void OnDestroy()
    {
        _databaseManager.DeleteServer("Bombertale");
    }

    void Update()
    {
        ReceiveEvent recEvent = base.Receive();
        switch (recEvent.type)
        {
            case NetworkEventType.ConnectEvent:
                clientList.Add(recEvent.sender);
                _databaseManager.UpdatePlayers("Bombertale", clientList.Count);
                SendAll(MessageType.LobbyUpdate, clientList.Count.ToString());
                break;
        }
        //int connectionID;
        //int channelID;
        //byte[] recBuffer = new byte[1024];
        //int bufferSize = 1024;
        //int dataSize;
        //byte error;
        //NetworkEventType recData = NetworkTransport.ReceiveFromHost(this._hostID, out connectionID, out channelID, recBuffer, bufferSize, out dataSize, out error);
        //if (recData == NetworkEventType.ConnectEvent)
        //{
        //    Debug.Log("New connection: " + connectionID);
        //    clientList.Add(connectionID);
        //    _databaseManager.UpdatePlayers("Bombertale", clientList.Count);
        //    foreach (int clientID in clientList){
        //        NetworkTransport.Send(this._hostID, clientID, this._myReliableChannelID, 
        //            System.Text.Encoding.UTF8.GetBytes(clientList.Count.ToString()), 1024, out error);
        //    }            
        //}
    }

    public void SendAll(MessageType messageType, object data)
    {
        foreach (int i in clientList)
        {
            base.Send(i, messageType, data);
        }
    }
    //void LateUpdate()
    //{
    //    if (_playersInLobby != null)
    //    {
    //        _playersInLobby.text = "Players: " + this.clientList.Count + "/4";
    //    }
    //}
}
