using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ServerSelection : MonoBehaviour {

    private ServerListManager _serverListManager;
    private Text _serverName;

    void Awake()
    {
        _serverListManager = GameObject.Find("ServerListManager").GetComponent<ServerListManager>();
        _serverName = this.transform.FindChild("ServerName").GetComponent<Text>();
    }

    public void OnServerSelection()
    {
        Debug.Log(_serverListManager.servers[_serverName.text].IP);
        Debug.Log(_serverListManager.servers[_serverName.text].port);
    }
}
