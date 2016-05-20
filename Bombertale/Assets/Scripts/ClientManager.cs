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
    private AudioSource joinSound;
    public string[] clientUsernames;

    /// Lobby Variables
    private Text _playersInLobby, _serverName;

    /// In-Game Variables
    public string winnerMessage;
    private bool _isGameStarted = false;
    private bool _noneWasSent = false;
    NetworkPlayer myPlayer;
    private Dictionary<string, NetworkPlayer> _players = new Dictionary<string, NetworkPlayer>();
    public List<List<char>> charMap;
    public List<List<GameObject>> gameObjectMap;

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
        clientUsernames = new string[4];
        joinSound = GameObject.Find("LobbyAudioManager").GetComponent<AudioSource>();
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
            Debug.Log(message.subJson);

            switch (message.type)
            {
                case MessageType.UsernameRequest:
                    HandleUsernameRequest();
                    break;
                case MessageType.LobbyUpdate:
                    HandleLobbyUpdate(message);
                    break;
                case MessageType.StartGame:
                    HandleStartGame();
                    break;
                case MessageType.Setup:
                    HandleSetup(message);
                    break;
                case MessageType.StateUpdate:
                    HandleStateUpdate(message);
                    break;
                case MessageType.BombReply:
                    HandleBombReply(message);
                    break;
                //case MessageType.MoveReply:
                //    HandleMoveReply(message);
                //    break;
                case MessageType.DestroySoftBlock:
                    HandleDestroySoftBlock(message);
                    break;
                case MessageType.PowerUpDrop:
                    HandlePowerUpDrop(message);
                    break;
                case MessageType.TriggerReply:
                    HandleTriggerReply(message);
                    break;
                case MessageType.GameOver:
                    HandleGameOver(message);
                    break;
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
        if (Mapper.MapToString(charMap) == mapString)
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
                    char myCell = charMap[col][row];
                    if (myCell != newCell)
                    {
                        if (newCell == CellID.Empty)
                        {
                            //Destroy w/e I have
                            Destroy(gameObjectMap[col][row]);
                        }
                        else if (myCell == CellID.SoftBlock && newCell != CellID.SoftBlock)
                        {
                            Destroy(gameObjectMap[col][row]);
                            gameObjectMap[col][row] = Mapper.CreateCell(newCell, col, row);
                        }
                        else
                        {
                            //Instantiate
                            gameObjectMap[col][row] = Mapper.CreateCell(newCell, col, row);
                        }
                    }
                }
            }

            charMap = newMap;
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

            char cellChar = charMap[xLoc][yLoc];
            int triggerValue = (int)System.Char.GetNumericValue(cellChar);
            if (0 <= triggerValue && triggerValue <= 4) // 0:speed, 1:bombup, 2: explosionup, 3: determination, 4: explosion.... hardcoded... but commented code above is more flexible
            {
                SendTriggerRequest(cellChar);
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

    public void SendTriggerRequest(char cellID)
    {
        base.Send(_server, MessageType.TriggerRequest, new TriggerRequest(cellID));
    }

    private void HandleUsernameRequest()
    {
        //efren's code touch all you want
        if (UIManager.userName == "")
            base.Send(_server, MessageType.UsernameReply, new UsernameReply("Frisk"));
        else
            base.Send(_server, MessageType.UsernameReply, new UsernameReply(UIManager.userName));
    }

    private void HandleLobbyUpdate(Message message)
    {
        LobbyUpdate lobbyUpdate = (LobbyUpdate)message.GetData();
        _playerCount = lobbyUpdate.playerCount;
        clientUsernames = lobbyUpdate.usernames;
        for (int i = 0; i < clientUsernames.Length; i++)
        {
            if (clientUsernames[i] == null || clientUsernames[i] == "[Joinable]" || clientUsernames[i] == "")
                clientUsernames[i] = "[Joinable]";
            else
            {
                GameObject.Find("Player" + (i + 1).ToString()).GetComponent<UnityEngine.UI.Text>().text = clientUsernames[i];
                GameObject.Find("Player" + (i + 1).ToString() + " Avatar").GetComponent<Animator>().SetBool("Joined", true);
                joinSound.Play();
            }
        }
    }

    private void HandleStartGame()
    {
        if (this.GetComponent<ServerManager>() != null)
            SceneManager.LoadScene("ServerGame");
        else
            SceneManager.LoadScene("ClientGame");
    }

    private void HandleSetup(Message message)
    {
        Setup setup = (Setup)message.GetData();
        charMap = Mapper.StringToMap(Mapper.mapString);
        gameObjectMap = Mapper.CreateGameObjectMap(charMap);

        foreach (string playerName in setup.players)
        {
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
        GameAudio _gameAudio = GameObject.Find("GameAudioManager").GetComponent<GameAudio>();
        pickupSound = _gameAudio.pickupSound;
        deathSound = _gameAudio.deathSound;
        _gameAudio.SelectMusic(setup.songSelection);
        this._isGameStarted = true;
    }

    private void HandleStateUpdate(Message message)
    {
        StateUpdate stateUpdate = (StateUpdate)message.GetData();
        SyncMap(stateUpdate.mapString);
        foreach (PlayerData serverPlayer in stateUpdate.players)
        {
            NetworkPlayer player = _players[serverPlayer.name];
            if (player.data.gridLocation != serverPlayer.gridLocation)
            {
                player.GetComponent<Rigidbody2D>().MovePosition(new Vector2(serverPlayer.gridLocation.x + .5f, serverPlayer.gridLocation.y + .5f));
            }
            player.data.direction = serverPlayer.direction;
        }
    }

    private void HandleBombReply(Message message)
    {
        BombReply bombReply = (BombReply)message.GetData();
        NetworkPlayer droppingPlayer = _players[bombReply.playerData.name];
        droppingPlayer.DropBomb();
    }

    private void HandleMoveReply(Message message)
    {
        MoveReply moveReply = (MoveReply)message.GetData();
        NetworkPlayer playerToMove = _players[moveReply.playerName];
        if (playerToMove.GetGridLocation() != moveReply.gridLocation)
        {
            playerToMove.GetComponent<Rigidbody2D>().MovePosition(new Vector2(moveReply.gridLocation.x + .5f, moveReply.gridLocation.y + .5f));
        }
        playerToMove.data.direction = moveReply.moveDir;
    }

    private void HandleDestroySoftBlock(Message message)
    {
        DestroySoftBlock rekt = (DestroySoftBlock)message.GetData();
        GameObject softBlockToDestroy = gameObjectMap[rekt.xLoc][rekt.yLoc];
        if (softBlockToDestroy != null)
            softBlockToDestroy.GetComponent<NetworkSoftBlock>().Fizzle();
    }

    private void HandlePowerUpDrop(Message message)
    {
        PowerUpDrop puDrop = (PowerUpDrop)message.GetData();

        charMap[puDrop.xLoc][puDrop.yLoc] = (puDrop.puIndex == -1) ? CellID.Empty : puDrop.puIndex.ToString()[0];
    }

    private void HandleTriggerReply(Message message)
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
        if (charMap[triggerReply.xLoc][triggerReply.yLoc] != CellID.Explosion)
            charMap[triggerReply.xLoc][triggerReply.yLoc] = CellID.Empty;
        Destroy(gameObjectMap[triggerReply.xLoc][triggerReply.yLoc]);
    }

    private void HandleGameOver(Message message)
    {
        GameOver gameOver = (GameOver)message.GetData();
        this.winnerMessage = gameOver.winnerMessage;
        _isGameStarted = false;
        SceneManager.LoadScene("NetworkEndScreen");
    }
}
