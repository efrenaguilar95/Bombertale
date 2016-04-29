using UnityEngine;
using System.Collections;

[System.Serializable]
public struct LobbyUpdate
{
    public int playerCount;
    public LobbyUpdate(int playerCount)
    {
        this.playerCount = playerCount;
    }
}

public enum MessageType
{
    None,
    LobbyUpdate
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
            default:
                return null;
        }
    }
}
