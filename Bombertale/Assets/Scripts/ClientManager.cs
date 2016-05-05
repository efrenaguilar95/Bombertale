using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ClientManager : NetworkHost
{
    public GameObject[] powerUps;
    private int _server;
    private int _playerCount;
    
    /// Lobby Variables
    private Text _playersInLobby;

    /// In-Game Variables
    private bool _isGameStarted = false;
    private bool _noneWasSent = false;    
    NetworkPlayer myPlayer;
    private Dictionary<string, NetworkPlayer> _players = new Dictionary<string, NetworkPlayer>();    
    private Mapper map;

    public AudioSource bombSound;
    public AudioSource deathSound;
    public AudioSource pickupSound;

    void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name != "ClientLobby" && SceneManager.GetActiveScene().name != "ClientGame" && SceneManager.GetActiveScene().name != "ServerLobby" && SceneManager.GetActiveScene().name != "ServerGame")
        {
            NetworkTransport.Shutdown();
            Destroy(this.gameObject);
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        int randomPort = Random.Range(10000, 65000);
        base.Setup(randomPort, 1);
        _server = base.Connect(NetworkHost.ServerIP, NetworkHost.Port);

        _playersInLobby = GameObject.Find("PlayersInLobbyText").GetComponent<Text>();

    }


    void Update()
    {
        PollMovement();
        ReceiveEvent recEvent = base.Receive();
        if (recEvent.type == NetworkEventType.DisconnectEvent)
        {
            Debug.Log("Server kicked me");
            SceneManager.LoadScene("ServerList");
        }
        if (recEvent.type == NetworkEventType.DataEvent)
        {
            Message message = recEvent.message;
            //Debug.Log(message.subJson);
            if (message.type == MessageType.LobbyUpdate)
            {
                LobbyUpdate lobbyUpdate = (LobbyUpdate)message.GetData();
                _playerCount = lobbyUpdate.playerCount;
            }
            if (message.type == MessageType.StartGame)
            {
                this._isGameStarted = true;
                if (this.GetComponent<ServerManager>() != null)
                    SceneManager.LoadScene("ServerGame");
                else
                    SceneManager.LoadScene("ClientGame");
            }
            if (message.type == MessageType.Setup)
            {
                Setup setup = (Setup)message.GetData();
                map = GameObject.Find("Map").GetComponent<Mapper>();                
                foreach (string playerName in setup.players)
                {
                    //_players.Add(GameObject.Find(playerName).GetComponent<NetworkPlayer>());
                    _players.Add(playerName, GameObject.Find(playerName).GetComponent<NetworkPlayer>());
                }
                for (int i = 1; i <= 4; i++)
                {
                    NetworkPlayer player = GameObject.Find("Player" + i).GetComponent<NetworkPlayer>();
                    if (_players.ContainsKey(player.data.name)){
                        player.data.isAlive = true;
                    }
                    else{
                        player.gameObject.SetActive(false);
                    }
                }
                myPlayer = _players[setup.players[0]];
                pickupSound = GameObject.Find("GameAudioManager").GetComponent<GameAudio>().pickupSound;
                deathSound = GameObject.Find("GameAudioManager").GetComponent<GameAudio>().deathSound;
            }
            //if (message.type == MessageType.StateUpdate)
            //{
            //    StateUpdate stateUpdate = (StateUpdate)message.GetData();
            //    //for (int i = 0; i < stateUpdate.players.Count; i++)
            //    foreach (PlayerData playerData in stateUpdate.players)
            //    {
            //        _players[playerData.name].GetComponent<Rigidbody2D>().MovePosition(playerData.worldLocation);
            //        //_players[i].GetComponent<Rigidbody2D>().MovePosition(stateUpdate.players[i].worldLocation);
            //    }
            //    /// Need to set all the other data variables like powerups here as well to stay in sync with the server
            //}
            if (message.type == MessageType.BombReply)
            {
                BombReply bombReply = (BombReply)message.GetData();
                NetworkPlayer droppingPlayer = _players[bombReply.playerData.name];
                droppingPlayer.DropBomb();
            }
            if (message.type == MessageType.MoveReply)
            {
                MoveReply moveReply = (MoveReply)message.GetData();
                NetworkPlayer playerToMove = _players[moveReply.playerName];
                if (playerToMove.GetGridLocation() != moveReply.gridLocation)
                {
                    playerToMove.GetComponent<Rigidbody2D>().MovePosition(new Vector2(moveReply.gridLocation.x + .5f, moveReply.gridLocation.y + .5f));
                }
                playerToMove.data.direction = moveReply.moveDir;
            }
            if (message.type == MessageType.DestroySoftBlock)
            {
                DestroySoftBlock rekt = (DestroySoftBlock)message.GetData();
                GameObject softBlockToDestroy = map.gameObjectGrid[rekt.xLoc][rekt.yLoc];
                if (softBlockToDestroy != null)
                    softBlockToDestroy.GetComponent<NetworkSoftBlock>().Fizzle();
            }
            if (message.type == MessageType.PowerUpDrop)
            {
                PowerUpDrop puDrop = (PowerUpDrop)message.GetData();
                if (puDrop.puIndex == -1)
                {
                    map.grid[puDrop.xLoc][puDrop.yLoc] = ".";
                }
                else
                {
                    GameObject power = this.powerUps[puDrop.puIndex];
                    GameObject newPowerUp = (GameObject)Instantiate(power, new Vector2(puDrop.xLoc, puDrop.yLoc), Quaternion.identity);
                    map.grid[puDrop.xLoc][puDrop.yLoc] = puDrop.puIndex.ToString();
                    map.gameObjectGrid[puDrop.xLoc][puDrop.yLoc] = newPowerUp;
                }
            }
            if (message.type == MessageType.TriggerReply)
            {
                TriggerReply triggerReply = (TriggerReply)message.GetData();
                _players[triggerReply.playerData.name].data = triggerReply.playerData;
                if (triggerReply.playerData.isAlive == false)
                {
                    _players[triggerReply.playerData.name].gameObject.SetActive(false);
                    deathSound.Play();
                    GameObject napstablook = (GameObject)Instantiate(Resources.Load("Napstablook"),triggerReply.playerData.worldLocation, Quaternion.identity);
                    Destroy(napstablook, 1.5f);
                }
                else
                {
                    pickupSound.Play();
                }
                Destroy(map.gameObjectGrid[triggerReply.xLoc][triggerReply.yLoc]);
            }
        }
    }

    void LateUpdate()
    {
        if (_playersInLobby != null)
            _playersInLobby.text = "Players: " + this._playerCount + "/4";
        foreach (NetworkPlayer player in _players.Values)
        {
            MovePlayer(player);
        }
    }

    private void PollMovement()
    {
        if (_isGameStarted)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                base.Send(_server, MessageType.Move, new Move(Direction.UP));
                _noneWasSent = false;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                base.Send(_server, MessageType.Move, new Move(Direction.LEFT));
                _noneWasSent = false;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                base.Send(_server, MessageType.Move, new Move(Direction.DOWN));
                _noneWasSent = false;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                base.Send(_server, MessageType.Move, new Move(Direction.RIGHT));
                _noneWasSent = false;
            }

            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
            {
                //Used to prevent spamming the server.
                if (!_noneWasSent)
                {
                    base.Send(_server, MessageType.Move, new Move(Direction.NONE));                    
                    _noneWasSent = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                base.Send(_server, MessageType.BombRequest, new BombRequest());
            }
        }
    }

    private void MovePlayer(NetworkPlayer player)
    {
        switch (player.data.direction)
        {
            case Direction.UP:
                player.transform.Translate(new Vector2(0, player.data.speed * Time.deltaTime));
                break;
            case Direction.LEFT:
                player.transform.Translate(new Vector2(-player.data.speed * Time.deltaTime, 0));
                break;
            case Direction.DOWN:
                player.transform.Translate(new Vector2(0, -player.data.speed * Time.deltaTime));
                break;
            case Direction.RIGHT:
                player.transform.Translate(new Vector2(player.data.speed * Time.deltaTime, 0));
                break;
            default:
                break;
        }
    }

    public void SendTriggerRequest(TriggerType triggerType)
    {
        base.Send(_server, MessageType.TriggerRequest, new TriggerRequest(triggerType));
    }
}
