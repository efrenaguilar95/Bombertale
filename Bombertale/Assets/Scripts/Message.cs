using UnityEngine;
using System.Collections;

public enum MessageType
{
    None,
    LobbyUpdate,
    Setup,
    Move
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
public class Message{
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
            default:
                return null;
        }
    }
}
