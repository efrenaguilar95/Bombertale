﻿using UnityEngine;
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
    private Text _playersInLobby, _serverName;

    /// In-Game Variables
    public string winnerMessage;
    private bool _isGameStarted = false;
    private bool _noneWasSent = false;
    NetworkPlayer myPlayer;
    private Dictionary<string, NetworkPlayer> _players = new Dictionary<string, NetworkPlayer>();
    //private List<List<bool>> powerupSentHistory = new List<List<bool>>();
    //private bool[][] powerupSentHistory = null;
    private Mapper map;

    private AudioSource bombSound;
    private AudioSource deathSound;
    private AudioSource pickupSound;

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
        _serverName = GameObject.Find("ServerName").GetComponent<Text>();
        if (_serverName != null)
            _serverName.text = "Lobby: " + NetworkHost.ServerName;
    }


    void Update()
    {
        PollMovement();
        CheckForCollisions();
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
            if (message.type == MessageType.UsernameRequest)
            {
                base.Send(_server, MessageType.UsernameReply, new UsernameReply(UIManager.userName));
            }
            if (message.type == MessageType.LobbyUpdate)
            {
                LobbyUpdate lobbyUpdate = (LobbyUpdate)message.GetData();
                _playerCount = lobbyUpdate.playerCount;
                for (int i = 0; i < lobbyUpdate.usernames.Length; i++)
                {
                    Debug.Log(i.ToString() + ":" + lobbyUpdate.usernames[i]);
                }
            }
            if (message.type == MessageType.StartGame)
            {
                if (this.GetComponent<ServerManager>() != null)
                    SceneManager.LoadScene("ServerGame");
                else
                    SceneManager.LoadScene("ClientGame");
            }
            if (message.type == MessageType.Setup)
            {
                Setup setup = (Setup)message.GetData();
                map = GameObject.Find("Map").GetComponent<Mapper>();

                //for (int col = 0; col < map.grid.Count; col++)
                //{
                //    powerupSentHistory.Add(new List<bool>());
                //    for (int row = 0; row < map.grid[col].Count; row++)
                //        powerupSentHistory[col].Add(false);
                //}

                foreach (string playerName in setup.players)
                {
                    //_players.Add(GameObject.Find(playerName).GetComponent<NetworkPlayer>());
                    _players.Add(playerName, GameObject.Find(playerName).GetComponent<NetworkPlayer>());
                }
                for (int i = 1; i <= 4; i++)
                {
                    NetworkPlayer player = GameObject.Find("Player" + i).GetComponent<NetworkPlayer>();
                    if (_players.ContainsKey(player.data.name))
                    {
                        player.data.isAlive = true;
                    }
                    else
                    {
                        player.gameObject.SetActive(false);
                    }
                }
                myPlayer = _players[setup.players[0]];
                pickupSound = GameObject.Find("GameAudioManager").GetComponent<GameAudio>().pickupSound;
                deathSound = GameObject.Find("GameAudioManager").GetComponent<GameAudio>().deathSound;
                GameObject.Find("GameAudioManager").GetComponent<GameAudio>().SelectMusic(setup.songSelection);
                this._isGameStarted = true;
            }

            if (message.type == MessageType.StateUpdate)
            {
                StateUpdate stateUpdate = (StateUpdate)message.GetData();
                SyncMap(stateUpdate.mapString);
            }

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
                    map.grid[puDrop.xLoc][puDrop.yLoc] = CellID.Empty;
                }
                else
                {
                    GameObject power = this.powerUps[puDrop.puIndex];
                    GameObject newPowerUp = (GameObject)Instantiate(power, new Vector2(puDrop.xLoc, puDrop.yLoc), Quaternion.identity);
                    map.grid[puDrop.xLoc][puDrop.yLoc] = puDrop.puIndex.ToString()[0];
                    map.gameObjectGrid[puDrop.xLoc][puDrop.yLoc] = newPowerUp;
                }
            }
            if (message.type == MessageType.TriggerReply)
            {
                TriggerReply triggerReply = (TriggerReply)message.GetData();
                NetworkPlayer triggeredPlayer = _players[triggerReply.playerData.name];
                triggeredPlayer.data = triggerReply.playerData;
                if (triggerReply.playerData.isAlive == false)
                {
                    _players[triggerReply.playerData.name].gameObject.SetActive(false);
                    deathSound.Play();
                    GameObject napstablook = (GameObject)Instantiate(Resources.Load("Napstablook"), triggerReply.playerData.worldLocation, Quaternion.identity);
                    Destroy(napstablook, 1.5f);
                }
                else
                {
                    pickupSound.Play();
                }
                if (map.grid[triggerReply.xLoc][triggerReply.yLoc] != CellID.Explosion)
                    map.grid[triggerReply.xLoc][triggerReply.yLoc] = CellID.Empty;
                Destroy(map.gameObjectGrid[triggerReply.xLoc][triggerReply.yLoc]);
                //if (triggeredPlayer == myPlayer)
                //    powerupSentHistory[triggerReply.xLoc][triggerReply.yLoc] = false;
            }
            if (message.type == MessageType.GameOver)
            {
                GameOver gameOver = (GameOver)message.GetData();
                this.winnerMessage = gameOver.winnerMessage;
                _isGameStarted = false;
                SceneManager.LoadScene("NetworkEndScreen");
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

    private void SyncMap(string mapString)
    {
        if (Mapper.MapToString(map.grid) == mapString)
        {
            Debug.Log("No changes to map");
            return;
        }
        else
        {
            Debug.Log("Changes to map");
            List<List<char>> newMap = Mapper.StringToMap(mapString);
            for (int col = 0; col < newMap.Count; col++)
            {
                for (int row = 0; row < newMap[col].Count; row++)
                {
                    char newCell = newMap[col][row];
                    char myCell = map.grid[col][row];
                    if (myCell == CellID.Empty && newCell != CellID.Empty)
                    {
                        //Instantiate
                    }
                    else if (newCell == CellID.Empty && myCell != CellID.Empty)
                    {
                        //Delete whatever object I have here
                    }
                }
            }
        }
    }

    private void PollMovement()
    {
        if (_isGameStarted && myPlayer.data.isAlive)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                base.Send(_server, MessageType.Move, new Move(Direction.UP));
                myPlayer.data.direction = Direction.UP;
                _noneWasSent = false;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                base.Send(_server, MessageType.Move, new Move(Direction.LEFT));
                myPlayer.data.direction = Direction.LEFT;
                _noneWasSent = false;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                base.Send(_server, MessageType.Move, new Move(Direction.DOWN));
                myPlayer.data.direction = Direction.DOWN;
                _noneWasSent = false;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                base.Send(_server, MessageType.Move, new Move(Direction.RIGHT));
                myPlayer.data.direction = Direction.RIGHT;
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

    private void CheckForCollisions()
    {
        if (_isGameStarted && myPlayer.data.isAlive)
        {
            Vector2 myGridLoc = myPlayer.GetGridLocation();
            int xLoc = (int)myGridLoc.x;
            int yLoc = (int)myGridLoc.y;
            //if (!powerupSentHistory[xLoc][yLoc])
            //{
            char cellChar = map.grid[xLoc][yLoc];
            int triggerValue = (int)System.Char.GetNumericValue(cellChar);
            //bool success = int.TryParse(map.grid[(int)myGridLoc.x][(int)myGridLoc.y].ToString(), out triggerValue);
            if (0 <= triggerValue && triggerValue <= 4) // 0:speed, 1:bombup, 2: explosionup, 3: determination, 4: explosion.... hardcoded... but commented code above is more flexible
            {
                SendTriggerRequest(cellChar);
                //powerupSentHistory[xLoc][yLoc] = true;
            }
            //}
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

    public void SendTriggerRequest(char cellID)
    {
        //base.Send(_server, MessageType.TriggerRequest, new TriggerRequest(triggerType));
        base.Send(_server, MessageType.TriggerRequest, new TriggerRequest(cellID));
    }
}
