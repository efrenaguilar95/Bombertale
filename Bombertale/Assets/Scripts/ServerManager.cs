using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerManager : MonoBehaviour
{

    private List<Server> servers = new List<Server>();
    private string[] serverList;
    private int serverCount;
    private string unityPassword = "ICS168";

    struct Server
    {
        public string serverName;
        public string IP;
        public int port;
        public bool security;
        public string password;
        public int playerCount;
        public Server(string[] serverString)
        {
            serverName = serverString[0];
            IP = serverString[1];
            port = int.Parse(serverString[2]);
            security = (int.Parse(serverString[3])==1? true : false);
            password = serverString[4];
            playerCount = int.Parse(serverString[5]);
        }
    }

    public void Refresh()
    {
        splitString(GetServers());
        servers.Clear();
        for (int i = 0; i < serverCount; i++)
        {
            string[] temp;
            temp = serverList[i].Split('&');
            servers.Add(new Server(temp));
        }
        Debug.Log(servers[0].serverName);
    }

    private void splitString(string message)
    {
        string temp;
        temp = message.Substring(6, message.Length - 6);
        serverList = temp.Split('#');
        serverCount = serverList.Length;
    }

    public string GetServers()
    {
        string url = "http://apedestrian.com/bombertale/GetLobby.php?unityPassword=" + unityPassword;
        return GetText(url);
    }

    private string GetText(string url)
    {
        WWW www = new WWW(url.Replace(" ", "%20"));
        StartCoroutine(WaitForRequest(www));
        while (!www.isDone)
        {
            //...
        }
        return www.text;
    }

    private IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        //// check for errors
        //if (!string.IsNullOrEmpty(www.error))
        //    Debug.Log("WWW Error: " + www.error);
        //else
        //    Debug.Log("WWW Success!: " + www.text);
    }


}
