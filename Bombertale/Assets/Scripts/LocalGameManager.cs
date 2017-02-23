using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LocalGameManager : MonoBehaviour {

    public GameObject player1, player2, player3, player4; 
    struct PlayerStruct{
        public GameObject player;
        public LocalGamePlayer playerScript;
        public KeyCode upKey;
        public KeyCode leftKey;
        public KeyCode downKey;
        public KeyCode rightKey;
        public KeyCode bombKey;
        public bool isAlive;
        public PlayerStruct(GameObject player, KeyCode up, KeyCode left, KeyCode down, KeyCode right, KeyCode bomb)
        {
            this.player = player;
            playerScript = player.GetComponent<LocalGamePlayer>();
            upKey = up;
            downKey = down;
            rightKey = right;
            leftKey = left;
            bombKey = bomb;
            isAlive = true;
        }
    }
    PlayerStruct p1Struct;
    PlayerStruct p2Struct;
    PlayerStruct p3Struct;
    PlayerStruct p4Struct;

    public static int win = 0;

    public List<List<char>> charMap;
    public List<List<GameObject>> gameObjectMap;

    public string mapStringPub;
    private GameAudio _gameAudio;
    private int invlunCount = 0;


    void Awake() {
        p1Struct = new PlayerStruct(player1, KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space);
        p2Struct = new PlayerStruct(player2, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.Mouse0);
        p3Struct = new PlayerStruct(player3, KeyCode.Keypad5, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad0);
        p4Struct = new PlayerStruct(player4, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.P);
        _gameAudio = GameObject.Find("GameAudioManager").GetComponent<GameAudio>();
    }

    void Start()
    {
        charMap = LocalMapper.StringToMap(LocalMapper.mapString);
        mapStringPub = LocalMapper.mapString;
        gameObjectMap = LocalMapper.CreateGameObjectMap(charMap);
    }

    void Update()
    {
        UpdateMap(LocalMapper.mapString);
        mapStringPub = LocalMapper.MapToString(charMap);
        PlayerHandler(p1Struct);
        PlayerHandler(p2Struct);
        PlayerHandler(p3Struct);
        PlayerHandler(p4Struct);
        PlayersAlive();
        if (PlayersAlive() <= 1)
        {
            win = Winner();
            DeleteAll();
            SceneManager.LoadScene("LocalEndScreen");
        }
        
    }

    void LateUpdate()
    {
        invlunCount = 0;
        if (p1Struct.playerScript.isDetermined)
            invlunCount++;
        if (p2Struct.playerScript.isDetermined)
            invlunCount++;
        if (p3Struct.playerScript.isDetermined)
            invlunCount++;
        if (p4Struct.playerScript.isDetermined)
            invlunCount++;
        if (invlunCount == 0)
            useInvulnMusic(false);
        else
            useInvulnMusic(true);
    }

    void DeleteAll()
    {
        foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            Destroy(o);
        }
    }

    private int PlayersAlive()
    {
        int alive = 4;
        if (!p1Struct.playerScript.isAlive)
        {
            alive--;
        }
        if (!p2Struct.playerScript.isAlive)
        {
            alive--;
        }
        if (!p3Struct.playerScript.isAlive)
        {
            alive--;
        }
        if (!p4Struct.playerScript.isAlive)
        {
            alive--;
        }
        return alive;
    }

    private int Winner()
    {
        if (p1Struct.playerScript.isAlive)
        {
            return 1;
        }
        else if (p2Struct.playerScript.isAlive)
        {
            return 2;
        }
        else if (p3Struct.playerScript.isAlive)
        {
            return 3;
        }
        return 4;
    }

    void PlayerHandler(PlayerStruct pStruct)
    {
        if (!pStruct.isAlive)   //Have to do this wonky thing because the gameobject becomes disabled, disabling the script as well
            return;
        if (!pStruct.playerScript.isAlive)
        {
            pStruct.player.SetActive(false);
            pStruct.isAlive = false;
            return;
        }
        if (pStruct.playerScript.isDetermined)
        {
            invlunCount++;
        }
        else
        {
            invlunCount--;
        }
        //Movement
        if (Input.GetKeyDown(pStruct.upKey))
        {
            pStruct.playerScript.verticalMovement = Direction.UP;
            if (!pStruct.playerScript.isMoving())
                pStruct.playerScript.toggleMovement();
            pStruct.playerScript.SetAnim("WalkUp");
        }
        else if (Input.GetKeyDown(pStruct.downKey))
        {
            pStruct.playerScript.verticalMovement = Direction.DOWN;
            if (!pStruct.playerScript.isMoving())
                pStruct.playerScript.toggleMovement();
            pStruct.playerScript.SetAnim("WalkDown");
        }
        else if (!Input.GetKey(pStruct.upKey) && !Input.GetKey(pStruct.downKey))
        {
            pStruct.playerScript.verticalMovement = Direction.NONE;
        }
        if (Input.GetKeyDown(pStruct.leftKey))
        {
            pStruct.playerScript.horizontalMovement = Direction.LEFT;
            if (!pStruct.playerScript.isMoving())
                pStruct.playerScript.toggleMovement();
            pStruct.playerScript.SetAnim("WalkLeft");
        }
        else if (Input.GetKeyDown(pStruct.rightKey))
        {
            pStruct.playerScript.horizontalMovement = Direction.RIGHT;
            if (!pStruct.playerScript.isMoving())
                pStruct.playerScript.toggleMovement();
            pStruct.playerScript.SetAnim("WalkRight");
        }
        else if (!Input.GetKey(pStruct.leftKey) && !Input.GetKey(pStruct.rightKey))
        {
            pStruct.playerScript.horizontalMovement = Direction.NONE;
        }
        if (pStruct.playerScript.horizontalMovement == Direction.NONE && pStruct.playerScript.verticalMovement == Direction.NONE)
        {            
            if (pStruct.playerScript.isMoving())
                pStruct.playerScript.toggleMovement();
        }
        //Place Bomb
        if (Input.GetKeyDown(pStruct.bombKey))
        {
            pStruct.playerScript.DropBomb();
        }

    }

    private void UpdateMap(string mapString)
    {
        if (LocalMapper.MapToString(charMap) == mapString)
        {
            return;
        }
        else
        {
            List<List<char>> newMap = LocalMapper.StringToMap(mapString);
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
                            gameObjectMap[col][row] = LocalMapper.CreateCell(newCell, col, row);
                        }
                        else
                        {
                            //Instantiate
                            gameObjectMap[col][row] = LocalMapper.CreateCell(newCell, col, row);
                        }
                    }
                }
            }

            charMap = newMap;
        }
    }
    private void useInvulnMusic(bool yes)
    {
        if (_gameAudio != null)
        {
            if (yes)
            {
                _gameAudio.musicSource.Pause();
                if (!_gameAudio.invulnMusic.isPlaying)
                    _gameAudio.invulnMusic.Play();
            }
            else
            {
                _gameAudio.invulnMusic.Stop();
                _gameAudio.musicSource.UnPause();
            }
        }
    }
}


///CLG Below
/*
        //Player1 Movement
        if (Input.GetKeyDown(KeyCode.W))
        {
            p1Script.verticalMovement = Direction.UP;
            if (!p1Script.isMoving())
                p1Script.toggleMovement();
            p1Script.SetAnim("WalkUp");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            p1Script.verticalMovement = Direction.DOWN;
            if (!p1Script.isMoving())
                p1Script.toggleMovement();
            p1Script.SetAnim("WalkDown");
        }
        else if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)){
            p1Script.verticalMovement = Direction.NONE;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            p1Script.horizontalMovement = Direction.LEFT;
            if(!p1Script.isMoving())
                p1Script.toggleMovement();
            p1Script.SetAnim("WalkLeft");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            p1Script.horizontalMovement = Direction.RIGHT;
            if(!p1Script.isMoving())
                p1Script.toggleMovement();
            p1Script.SetAnim("WalkRight");
        }
        else if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            p1Script.horizontalMovement = Direction.NONE;
        }
        if (p1Script.horizontalMovement == Direction.NONE && p1Script.verticalMovement == Direction.NONE)
        {
            //p1Script.SetAnim("IdleDown");
            if(p1Script.isMoving())
                p1Script.toggleMovement();
        }
        //Player1 Bomb
        if (Input.GetKeyDown(KeyCode.Space))
        {
            p1Script.DropBomb();    
        }

        //Player2 Movement
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            p2Script.verticalMovement = Direction.UP;
            if (!p2Script.isMoving())
                p2Script.toggleMovement();
            p2Script.SetAnim("WalkUp");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            p2Script.verticalMovement = Direction.DOWN;
            if (!p2Script.isMoving())
                p2Script.toggleMovement();
            p2Script.SetAnim("WalkDown");
        }
        else if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            p2Script.verticalMovement = Direction.NONE;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            p2Script.horizontalMovement = Direction.LEFT;
            if (!p2Script.isMoving())
                p2Script.toggleMovement();
            p2Script.SetAnim("WalkLeft");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            p2Script.horizontalMovement = Direction.RIGHT;
            if (!p2Script.isMoving())
                p2Script.toggleMovement();
            p2Script.SetAnim("WalkRight");
        }
        else if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            p2Script.horizontalMovement = Direction.NONE;
        }

        if (p2Script.horizontalMovement == Direction.NONE && p2Script.verticalMovement == Direction.NONE)
        {
            if (p2Script.isMoving())
                p2Script.toggleMovement();
        }
        //Player2 bomb
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            p2Script.DropBomb();
        }

        //Player3 Movement
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            p3Script.verticalMovement = Direction.UP;
            if (!p3Script.isMoving())
                p3Script.toggleMovement();
            p3Script.SetAnim("WalkUp");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            p3Script.verticalMovement = Direction.DOWN;
            if (!p3Script.isMoving())
                p3Script.toggleMovement();
            p3Script.SetAnim("WalkDown");
        }
        else if (!Input.GetKey(KeyCode.Keypad5) && !Input.GetKey(KeyCode.Keypad2))
        {
            p3Script.verticalMovement = Direction.NONE;
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            p3Script.horizontalMovement = Direction.LEFT;
            if (!p3Script.isMoving())
                p3Script.toggleMovement();
            p3Script.SetAnim("WalkLeft");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            p3Script.horizontalMovement = Direction.RIGHT;
            if (!p3Script.isMoving())
                p3Script.toggleMovement();
            p3Script.SetAnim("WalkRight");
        }
        else if (!Input.GetKey(KeyCode.Keypad1) && !Input.GetKey(KeyCode.Keypad3))
        {
            p3Script.horizontalMovement = Direction.NONE;
        }

        if (p3Script.horizontalMovement == Direction.NONE && p3Script.verticalMovement == Direction.NONE)
        {
            if (p3Script.isMoving())
                p3Script.toggleMovement();
        }
        //Player3 bomb
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            p3Script.DropBomb();
        }
        //Player4 Movement
        if (Input.GetKeyDown(KeyCode.I))
        {
            p4Script.verticalMovement = Direction.UP;
            if (!p4Script.isMoving())
                p4Script.toggleMovement();
            p4Script.SetAnim("WalkUp");
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            p4Script.verticalMovement = Direction.DOWN;
            if (!p4Script.isMoving())
                p4Script.toggleMovement();
            p4Script.SetAnim("WalkDown");
        }
        else if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K))
        {
            p4Script.verticalMovement = Direction.NONE;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            p4Script.horizontalMovement = Direction.LEFT;
            if (!p4Script.isMoving())
                p4Script.toggleMovement();
            p4Script.SetAnim("WalkLeft");
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            p4Script.horizontalMovement = Direction.RIGHT;
            if (!p4Script.isMoving())
                p4Script.toggleMovement();
            p4Script.SetAnim("WalkRight");
        }
        else if (!Input.GetKey(KeyCode.J) && !Input.GetKey(KeyCode.L))
        {
            p4Script.horizontalMovement = Direction.NONE;
        }

        if (p4Script.horizontalMovement == Direction.NONE && p4Script.verticalMovement == Direction.NONE)
        {
            if (p4Script.isMoving())
                p4Script.toggleMovement();
        }
        //Player4 bomb
        if (Input.GetKeyDown(KeyCode.P))
        {
            p4Script.DropBomb();
        }
        */