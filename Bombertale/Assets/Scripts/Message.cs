﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MessageType
{
    None,
    LobbyUpdate,
    Setup,
    Move,
    StateUpdate
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
public struct Setup
{

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
        FillPlayers(playerDict);
    }

    private void FillPlayers(Dictionary<int, NetworkPlayer> playerDict)
    {
        foreach (NetworkPlayer player in playerDict.Values)
        {
            players.Add(new PlayerData(player.data.direction, player.data.isAlive, player.data.speed, player.data.bombCount, player.data.explosionRadius, player.data.isInvulnerable, player.data.invulnTimeRemaining));
        }
    }

    //public Dictionary<int, NetworkPlayer> players;

    //public List<int> _playersKeys = new List<int>();
    //public List<NetworkPlayer> _playersValues = new List<NetworkPlayer>();

    //public StateUpdate(Dictionary<int, NetworkPlayer> playerDict)
    //{
    //    this.players = playerDict;
    //}

    //public void OnBeforeSerialize()
    //{
    //    _playersKeys.Clear();
    //    _playersValues.Clear();
    //    foreach (var kvp in players)
    //    {
    //        _playersKeys.Add(kvp.Key);
    //        _playersValues.Add(kvp.Value);
    //    }
    //}

    //public void OnAfterDeserialize()
    //{
    //    players.Clear();
    //    for (int i = 0; i != Mathf.Min(_playersKeys.Count, _playersValues.Count); i++)
    //    {
    //        players.Add(_playersKeys[i], _playersValues[i]);
    //    }
    //}
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
            case MessageType.Setup:
                return JsonUtility.FromJson<Setup>(subJson);
            case MessageType.Move:
                return JsonUtility.FromJson<Move>(subJson);
            case MessageType.StateUpdate:
                return JsonUtility.FromJson<StateUpdate>(subJson);
            default:
                return null;
        }
    }
}
