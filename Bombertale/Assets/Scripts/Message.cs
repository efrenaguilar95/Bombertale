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

[System.Serializable]
public struct Setup
{

}

public enum MessageType
{
    None,
    LobbyUpdate,
    Setup
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
            default:
                return null;
        }
    }
}
