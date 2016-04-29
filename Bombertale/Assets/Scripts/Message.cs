using UnityEngine;
using System.Collections;
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
                return JsonUtility.FromJson<int>(subJson);
            default:
                return null;
        }
    }
}
