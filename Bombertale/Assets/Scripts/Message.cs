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
    StateUpdate,
    BombRequest
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
public struct StateUpdate
{    
    public List<PlayerData> players;

    public StateUpdate(Dictionary<int, NetworkPlayer> playerDict)
    {
        this.players = new List<PlayerData>();
        foreach (NetworkPlayer player in playerDict.Values)
        {
            players.Add(new PlayerData(player.data.worldLocation, player.data.direction, player.data.isAlive, player.data.speed, player.data.bombCount, player.data.explosionRadius, player.data.isInvulnerable, player.data.invulnTimeRemaining));
        }
    }  
}

[System.Serializable]
public struct BombRequest
{
    
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
            case MessageType.StateUpdate:
                return JsonUtility.FromJson<StateUpdate>(subJson);
            case MessageType.BombRequest:
                return JsonUtility.FromJson<BombRequest>(subJson);
            default:
                return null;
        }
    }
}
