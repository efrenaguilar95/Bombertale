using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ServerSelection : MonoBehaviour {

    private ServerListManager _serverListManager;
    private Text _serverNameObject;

    void Awake()
    {
        _serverListManager = GameObject.Find("ServerListManager").GetComponent<ServerListManager>();
        _serverNameObject = this.transform.FindChild("ServerName").GetComponent<Text>();
    }

    public void OnServerSelection()
    {
        Debug.Log(_serverListManager.servers[_serverNameObject.text].IP);
        Debug.Log(_serverListManager.servers[_serverNameObject.text].port);
        NetworkHost.ServerName = _serverNameObject.text;
        NetworkHost.ServerIP = _serverListManager.servers[_serverNameObject.text].IP;
        NetworkHost.Port = _serverListManager.servers[_serverNameObject.text].port;
    }
}
