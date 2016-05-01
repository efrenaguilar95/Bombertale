﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ServerManager : NetworkHost
{
    public List<int> clientList = new List<int>();
    public ServerGameManager serverGameManager;

    private DatabaseManager _databaseManager;

    void Awake()
    {
        NetworkHost.ServerIP = Network.player.ipAddress;    //Placeholder until we get UI on create game
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
            case NetworkEventType.DataEvent:
                Message message = recEvent.message;
                if (message.type == MessageType.Move)
                {
                    Move playerMove = (Move)message.GetData();
                    serverGameManager.SetPlayerDirection(recEvent.sender, playerMove.moveDir);
                    //Debug.Log(playerMove.moveDir);
                }
                break;
        }
    }

    void LateUpdate()
    {
        if (serverGameManager != null)
        {
            SendAll(MessageType.StateUpdate, new StateUpdate(serverGameManager.clientToPlayer));
        }
    }

    public void SendAll(MessageType messageType, object data)
    {
        foreach (int i in clientList)
        {
            base.Send(i, messageType, data);
        }
    }

    public void PressStartGame()
    {
        SendAll(MessageType.StartGame, new StartGame());
    }

    public void SendSetup()
    {
        List<string> playerListToSend = new List<string>();
        for (int i = 0; i < clientList.Count; i++)
        {            
            playerListToSend.Add("Player" + (i + 1));
        }
        SendAll(MessageType.Setup, new Setup(playerListToSend));
        //for (int i = 0; i < clientList.Count; i++)
        //{
        //    List<string> playerListToSend = new List<string>();
        //    playerListToSend.Add("Player"+(i+1));

        //    for (int j = 0; j < clientList.Count; j++)
        //    {
        //        if (j != i)
        //        {
        //            playerListToSend.Add("Player" + (j + 1));
        //        }
        //    }
        //    base.Send(clientList[i], MessageType.Setup, new Setup(playerListToSend));
        //}
    }
}
