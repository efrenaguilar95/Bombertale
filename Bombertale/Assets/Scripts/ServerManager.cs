using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//Take this out after inheritance
using UnityEngine.Networking;

public class ServerManager : NetworkHost {
    public List<int> clientList = new List<int>();

    private DatabaseManager _databaseManager;   

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        base.Setup(NetworkHost.Port, 4);
        _databaseManager = this.GetComponent<DatabaseManager>();
        _databaseManager.CreateServer("Bombertale", NetworkHost.ServerIP, NetworkHost.Port, false, "", clientList.Count);        
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
                Debug.Log("New connection: " + recEvent.sender);
                clientList.Add(recEvent.sender);
                _databaseManager.UpdatePlayers("Bombertale", clientList.Count);
                SendAll(MessageType.LobbyUpdate, new LobbyUpdate(clientList.Count));
                break;
        }
    }

    public void SendAll(MessageType messageType, object data)
    {
        foreach (int i in clientList)
        {
            base.Send(i, messageType, data);
        }
    }
}
