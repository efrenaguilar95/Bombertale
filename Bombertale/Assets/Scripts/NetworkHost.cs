using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkHost : MonoBehaviour {
    //Pre-set the ServerIP to connect to or host from as this machine's local IP
    public static string ServerIP = "127.0.0.1";
    public static int Port = 100;

    private ConnectionConfig _config;
    public int _myReliableChannelID;
    private HostTopology _topology;
    public int _hostID;

    public virtual void Awake()
    {
        ServerIP = Network.player.ipAddress;
    }

    public void Setup(int port, int maxConnections)
    {
        NetworkTransport.Init();
        _config = new ConnectionConfig();
        _myReliableChannelID = _config.AddChannel(QosType.Reliable);
        _topology = new HostTopology(_config, maxConnections);
        _hostID = NetworkTransport.AddHost(_topology, port);
    }

    public int Connect(string ipAddress, int port)
    {
        byte error;

        int connectionID = NetworkTransport.Connect(_hostID, ipAddress, port, 0, out error);
        if (error == 0)
        {
            return connectionID;
        }
        else
        {
            return -1;
        }
    }
}
