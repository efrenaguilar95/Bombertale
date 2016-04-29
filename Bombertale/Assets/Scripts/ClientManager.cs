using UnityEngine;
using System.Collections;

public class ClientManager : NetworkHost
{
    private int _server;

    void Awake()
    {
        int randomPort = Random.Range(10000, 65000);
        base.Setup(randomPort, 1);
        _server = base.Connect(NetworkHost.ServerIP, NetworkHost.Port);
    }
}
