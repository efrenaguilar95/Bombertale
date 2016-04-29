using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkHost : MonoBehaviour {
    //Pre-set the ServerIP to connect to or host from as this machine's local IP
    public static string ServerIP = Network.player.ipAddress;
    public static int Port = 100;

    private ConnectionConfig _config;
    private int _myReliableChannelID;
    private HostTopology _topology;
    private int _hostID;

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
