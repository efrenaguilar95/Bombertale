using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public struct ReceiveEvent
{
    public NetworkEventType type;
    public int sender;
    public Message message;

    public ReceiveEvent(NetworkEventType type, int connectionID, byte[] message)
    {
        this.type = type;
        this.sender = connectionID;
        if (type != NetworkEventType.DataEvent)
            this.message = new Message();
        else
            this.message = JsonUtility.FromJson<Message>(System.Text.Encoding.UTF8.GetString(message));
    }
}

public class NetworkHost : MonoBehaviour {
    //Pre-set the ServerIP to connect to or host from as this machine's local IP
    public static string ServerIP = "";
    public static int Port = 0;
    public static string ServerName = "Bombertale";
    public static string ServerPassword = "";
    public static bool IsPrivate = false;

    private ConnectionConfig _config;
    public int _myReliableChannelID;
    private HostTopology _topology;
    public int _hostID;
    private int bufferSize = 1024;

    //public virtual void Awake()
    //{
    //    ServerIP = Network.player.ipAddress;
    //}

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

    public ReceiveEvent Receive()
    {
        int connectionID;
        int channelID;
        byte[] recBuffer = new byte[bufferSize];
        int dataSize;
        byte error;
        NetworkEventType recData = NetworkTransport.ReceiveFromHost(this._hostID, out connectionID, out channelID, recBuffer, bufferSize, out dataSize, out error);
        return new ReceiveEvent(recData, connectionID, recBuffer);
    }

    public void Send(int connectionID, MessageType messageType, object data)
    {
        string message = JsonUtility.ToJson(new Message(messageType, data));
        byte error;
        NetworkTransport.Send(_hostID, connectionID, _myReliableChannelID, System.Text.Encoding.UTF8.GetBytes(message), bufferSize, out error);
    }
}
