using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerManager : NetworkHost {
    public List<int> clientList = new List<int>();

    private DatabaseManager _databaseManager;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        base.Setup(NetworkHost.Port, 4);
        _databaseManager = this.GetComponent<DatabaseManager>();
        _databaseManager.CreateServer("Bombertale", NetworkHost.ServerIP, NetworkHost.Port, false, "", 1);
    }

    void OnDestroy()
    {
        _databaseManager.DeleteServer("Bombertale");
    }
}
