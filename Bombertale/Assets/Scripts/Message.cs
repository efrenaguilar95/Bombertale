using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MessageType
{
    None,
    LobbyUpdate,
    StartGame,
    Setup,
    Move,
    MoveReply,
    StateUpdate,
    BombRequest,
    BombReply,
    DestroySoftBlock,
    PowerUpDrop
}

[System.Serializable]
public struct LobbyUpdate
{
    public int playerCount;
    public LobbyUpdate(int playerCount)
    {
        this.playerCount = playerCount;
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
    public Setup(List<string> players)
    {
        this.players = players;
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
    public MoveReply(string playerName, Direction direction)
    {
        this.playerName = playerName;
        this.moveDir = direction;
    }
}

[System.Serializable]
public struct StateUpdate
{    
    public List<PlayerData> players;

    public StateUpdate(Dictionary<int, NetworkPlayer> playerDict)
    {
        this.players = new List<PlayerData>();
        foreach (NetworkPlayer player in playerDict.Values)
        {
            //players.Add(new PlayerData(player.data.name, player.data.worldLocation, player.data.direction, player.data.isAlive, 
            //    player.data.speed, player.data.bombCount, player.data.explosionRadius, player.data.isInvulnerable, player.data.invulnTimeRemaining));
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
            default:
                return null;
        }
    }
}
