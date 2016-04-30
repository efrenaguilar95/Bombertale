using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerGameManager : MonoBehaviour
{

    public Dictionary<int, NetworkPlayer> clientToPlayer = new Dictionary<int, NetworkPlayer>();

    private ServerManager _serverManager;
    private List<NetworkPlayer> _playerList = new List<NetworkPlayer>();

    void Awake()
    {
        for (int i = 1; i <= 4; i++)
            _playerList.Add(GameObject.Find("Player" + i).GetComponent<NetworkPlayer>());
        //_playerList.Add(GameObject.Find("Player1").GetComponent<NetworkPlayer>());
        //_playerList.Add(GameObject.Find("Player2").GetComponent<NetworkPlayer>());
        //_playerList.Add(GameObject.Find("Player3").GetComponent<NetworkPlayer>());
        //_playerList.Add(GameObject.Find("Player4").GetComponent<NetworkPlayer>());
        _serverManager = GameObject.Find("ServerManager").GetComponent<ServerManager>();
        _serverManager.serverGameManager = this;

    }

    void Start()
    {
        for (int i = 0; i < _serverManager.clientList.Count && i < 4; i++)
        {
            clientToPlayer.Add(_serverManager.clientList[i], _playerList[i]);
        }
    }

    void Update()
    {
        foreach (var kvp in clientToPlayer)
        {
            if (kvp.Value.direction != Direction.NONE)
                Debug.Log(kvp.Key + ": " + kvp.Value.direction);
        }
    }

    public void SetPlayerDirection(int clientID, Direction direction)
    {
        clientToPlayer[clientID].direction = direction;
    }
}