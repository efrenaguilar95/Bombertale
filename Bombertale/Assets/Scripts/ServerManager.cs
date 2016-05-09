using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ServerManager : NetworkHost
{
    public List<int> clientList = new List<int>();
    public string[] clientUsernames;
    public ServerGameManager serverGameManager;
    private AudioSource joinSound;

    private DatabaseManager _databaseManager;

    float randMusic;
    int indexMusic;

    void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name != "ServerLobby" && SceneManager.GetActiveScene().name != "ServerGame")
        {
            NetworkTransport.Shutdown();
            Debug.Log(SceneManager.GetActiveScene().name);
            Destroy(this.gameObject);
        }
    }

    void Awake()
    {
        NetworkHost.ServerIP = Network.player.ipAddress;    //Placeholder until we get UI on create game
        DontDestroyOnLoad(this.gameObject);
        base.Setup(NetworkHost.Port, 4);
        _databaseManager = this.GetComponent<DatabaseManager>();
        _databaseManager.CreateServer("Bombertale", NetworkHost.ServerIP, NetworkHost.Port, false, "", clientList.Count);   //Placeholder

        NetworkHost.ServerName = "Bombertale";  //Placeholder
        randMusic = Random.Range(0, 1000);
        indexMusic = Random.Range(0, 9);
        clientUsernames = new string[4];
        joinSound = GameObject.Find("LobbyAudioManager").GetComponent<AudioSource>();
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
                Debug.Log(recEvent.sender.ToString() + "Joined!");
                _databaseManager.UpdatePlayers("Bombertale", clientList.Count);
                SendAll(MessageType.UsernameRequest, new UsernameRequest());
                break;
            case NetworkEventType.DisconnectEvent:
                //Debug.Log("Connection lost: " + recEvent.sender);
                if (serverGameManager != null)
                {
                    NetworkPlayer disconnectedPlayer = serverGameManager.clientToPlayer[recEvent.sender];
                    disconnectedPlayer.data.isAlive = false;
                    int xLoc = (int)disconnectedPlayer.transform.position.x;
                    int yLoc = (int)disconnectedPlayer.transform.position.y;
                    SendAll(MessageType.TriggerReply, new TriggerReply(disconnectedPlayer.data, xLoc, yLoc));
                }
                clientList.Remove(recEvent.sender);
                //Debug.Log(recEvent.sender.ToString() + "Disconnected!");
                _databaseManager.UpdatePlayers("Bombertale", clientList.Count);
                clientUsernames[recEvent.sender - 1] = "";
                GameObject.Find("Player" + (recEvent.sender).ToString()).GetComponent<UnityEngine.UI.Text>().text = "[Joinable]";
                GameObject.Find("Player" + (recEvent.sender).ToString() + " Avatar").GetComponent<Animator>().SetBool("Joined", false);
                SendAll(MessageType.LobbyUpdate, new LobbyUpdate(clientList.Count, clientUsernames));
                break;
            case NetworkEventType.DataEvent:
                Message message = recEvent.message;
                if(message.type == MessageType.UsernameReply)
                {
                    UsernameReply username = (UsernameReply)message.GetData();
                    clientUsernames[recEvent.sender-1] = username.username;
                    SendAll(MessageType.LobbyUpdate, new LobbyUpdate(clientList.Count, clientUsernames));
                    for (int i = 0; i<clientUsernames.Length; i++)
                    {
                        if (clientUsernames[i] == null || clientUsernames[i] == "[Joinable]")
                            clientUsernames[i] = "[Joinable]";
                        else
                        {
                            if (clientUsernames[i] == "")
                                clientUsernames[i] = "Frisk";
                            GameObject.Find("Player" + (i + 1).ToString()).GetComponent<UnityEngine.UI.Text>().text = clientUsernames[i];
                            GameObject.Find("Player" + (i + 1).ToString() + " Avatar").GetComponent<Animator>().SetBool("Joined", true);
                            joinSound.Play();
                        }
                    }
                }
                if(message.type == MessageType.LobbyUpdate)
                {

                }
                if (message.type == MessageType.Move)
                {
                    Move playerMove = (Move)message.GetData();
                    serverGameManager.SetPlayerDirection(recEvent.sender, playerMove.moveDir);
                    NetworkPlayer movingPlayer = serverGameManager.clientToPlayer[recEvent.sender];
                    SendAll(MessageType.MoveReply, new MoveReply(movingPlayer.data.name, movingPlayer.data.direction, movingPlayer.GetGridLocation()));
                }
                if (message.type == MessageType.BombRequest)
                {
                    BombRequest bombRequest = (BombRequest)message.GetData();
                    if (serverGameManager.DropBomb(recEvent.sender))
                    {
                        NetworkPlayer droppingPlayer = serverGameManager.clientToPlayer[recEvent.sender];
                        //Vector2 bombLocation = droppingPlayer.GetGridLocation();
                        SendAll(MessageType.BombReply, new BombReply(droppingPlayer.data));
                    }
                }
                if (message.type == MessageType.TriggerRequest)
                {
                    TriggerRequest triggerRequest = (TriggerRequest)message.GetData();
                    NetworkPlayer triggeredPlayer = serverGameManager.clientToPlayer[recEvent.sender];
                    int x = (int)triggeredPlayer.transform.position.x;
                    int y = (int)triggeredPlayer.transform.position.y;
                    int triggerValue;
                    bool success = int.TryParse(serverGameManager.map.grid[x][y], out triggerValue);
                    if ((int)triggerRequest.triggerType == triggerValue)
                    {
                        serverGameManager.TriggerUpdate(recEvent.sender, triggerRequest.triggerType);
                        SendAll(MessageType.TriggerReply, new TriggerReply(triggeredPlayer.data, x, y));
                    }
                    //Debug.Log(new Vector2((int)triggeredPlayer.transform.position.x, (int)triggeredPlayer.transform.position.y));
                    //Debug.Log(serverGameManager.map.grid[(int)triggeredPlayer.transform.position.x][(int)triggeredPlayer.transform.position.y]);
                }
                break;
        }
    }

    void LateUpdate()
    {
        //if (serverGameManager != null)
        //{
        //    SendAll(MessageType.StateUpdate, new StateUpdate(serverGameManager.clientToPlayer));
        //}
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
        //List<string> playerListToSend = new List<string>();
        //for (int i = 0; i < clientList.Count; i++)
        //{            
        //    playerListToSend.Add("Player" + (i + 1));
        //}
        //SendAll(MessageType.Setup, new Setup(playerListToSend));
        if(randMusic <= 1)
        {
            Debug.Log(randMusic);
            indexMusic = 9;
        }
        for (int i = 0; i < clientList.Count; i++)
        {
            List<string> playerListToSend = new List<string>();
            playerListToSend.Add("Player" + (i + 1));

            for (int j = 0; j < clientList.Count; j++)
            {
                if (j != i)
                {
                    playerListToSend.Add("Player" + (j + 1));
                }
            }
            
            base.Send(clientList[i], MessageType.Setup, new Setup(playerListToSend, indexMusic));
        }
    }
}
