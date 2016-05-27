using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MessageType
{
    None,
    UsernameRequest,
    UsernameReply,
    LobbyUpdate,
    StartGame,
    Setup,
    Move,
    MoveReply,
    StateUpdate,
    BombRequest,
    BombReply,
    DestroySoftBlock,
    PowerUpDrop,
    TriggerRequest,
    TriggerReply,
    GameOver
}

[System.Serializable]
public struct UsernameRequest
{

}

[System.Serializable]
public struct UsernameReply
{
    public string username;
    public UsernameReply(string username)
    {
        this.username = username;
    }
}

[System.Serializable]
public struct LobbyUpdate
{
    public int playerCount;
    public string[] usernames;
    public LobbyUpdate(int playerCount, string[] usernames)
    {
        this.playerCount = playerCount;
        this.usernames = usernames;
    }
}

[System.Serializable]
public struct StartGame
{

}

[System.Serializable]
public struct Setup
{
    public List<string> players;
    public int songSelection;
    public float timestamp;

    public Setup(List<string> players, int songSelection, float serverTime)
    {
        this.players = players;
        this.songSelection = songSelection;
        this.timestamp = serverTime;
    }
}

[System.Serializable]
public struct Move
{
    public Direction moveDir;
    public Move(Direction direction)
    {
        this.moveDir = direction;
    }
}

[System.Serializable]
public struct MoveReply
{
    public string playerName;
    public Direction moveDir;
    public Vector2 gridLocation;

    public MoveReply(string playerName, Direction direction, Vector2 gridLoc)
    {
        this.playerName = playerName;
        this.moveDir = direction;
        this.gridLocation = gridLoc;
    }
}

[System.Serializable]
public struct StateUpdate
{
    public string mapString;
    public List<PlayerData> players;
    public float timeStamp;

    public StateUpdate(List<List<char>> map, Dictionary<int, NetworkPlayer> playerDict, float currentTime)
    {
        this.mapString = Mapper.MapToString(map);
        this.players = new List<PlayerData>();
        this.timeStamp = currentTime;
        foreach (NetworkPlayer player in playerDict.Values)
        {
            players.Add(new PlayerData(player.data));
        }
    }  
}

[System.Serializable]
public struct BombRequest
{
    
}

[System.Serializable]
public struct BombReply
{
    public PlayerData playerData;

    public BombReply(PlayerData playerData)
    {
        this.playerData = playerData;
    }
}

[System.Serializable]
public struct DestroySoftBlock
{
    public int xLoc, yLoc;
    public DestroySoftBlock(int xLoc, int yLoc)
    {
        this.xLoc = xLoc;
        this.yLoc = yLoc;
    }
}

[System.Serializable]
public struct PowerUpDrop
{
    public int puIndex, xLoc, yLoc;   
    public PowerUpDrop(int index, int x, int y)
    {
        this.puIndex = index;
        this.xLoc = x;
        this.yLoc = y;
    }
}

[System.Serializable]
public struct TriggerRequest
{
    //public CellID triggerType;
    public char cellId;
    public TriggerRequest(char id)
    {
        //this.triggerType = type;
        this.cellId = id;
    }
}

[System.Serializable]
public struct TriggerReply
{
    public PlayerData playerData;
    public int xLoc, yLoc;
    public TriggerReply(PlayerData data, int x, int y)
    {
        this.playerData = data;
        this.xLoc = x;
        this.yLoc = y;
    }
}

[System.Serializable]
public struct GameOver
{
    public string winnerMessage;
    public GameOver(string name)
    {
        this.winnerMessage = name;
    }
}

[System.Serializable]
public class Message
{
    public MessageType type;
    public string subJson;


    public Message(MessageType type = MessageType.None, object data = null)
    {
        this.type = type;
        this.subJson = JsonUtility.ToJson(data);
    }

    public object GetData()
    {
        switch (this.type)
        {
            case MessageType.UsernameReply:
                return JsonUtility.FromJson<UsernameReply>(subJson);
            case MessageType.LobbyUpdate:
                return JsonUtility.FromJson<LobbyUpdate>(subJson);
            case MessageType.StartGame:
                return JsonUtility.FromJson<StartGame>(subJson);
            case MessageType.Setup:
                return JsonUtility.FromJson<Setup>(subJson);
            case MessageType.Move:
                return JsonUtility.FromJson<Move>(subJson);
            case MessageType.MoveReply:
                return JsonUtility.FromJson<MoveReply>(subJson);
            case MessageType.StateUpdate:
                return JsonUtility.FromJson<StateUpdate>(subJson);
            case MessageType.BombRequest:
                return JsonUtility.FromJson<BombRequest>(subJson);
            case MessageType.BombReply:
                return JsonUtility.FromJson<BombReply>(subJson);
            case MessageType.DestroySoftBlock:
                return JsonUtility.FromJson<DestroySoftBlock>(subJson);
            case MessageType.PowerUpDrop:
                return JsonUtility.FromJson<PowerUpDrop>(subJson);
            case MessageType.TriggerRequest:
                return JsonUtility.FromJson<TriggerRequest>(subJson);
            case MessageType.TriggerReply:
                return JsonUtility.FromJson<TriggerReply>(subJson);
            case MessageType.GameOver:
                return JsonUtility.FromJson<GameOver>(subJson);
            default:
                return null;
        }
    }
}
