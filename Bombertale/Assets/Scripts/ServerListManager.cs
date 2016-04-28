using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ServerListManager : MonoBehaviour
{
    public GameObject serverRowPrefab;
    public Dictionary<string, Server> servers = new Dictionary<string, Server>();

    //private List<Server> servers = new List<Server>();    
    private string[] serverList;
    private int serverCount;
    private string unityPassword = "ICS168";

    List<GameObject> toDelete = new List<GameObject>();

    public struct Server
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
            security = (int.Parse(serverString[3]) == 1 ? true : false);
            password = serverString[4];
            playerCount = int.Parse(serverString[5]);
        }
    }

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        splitString(GetServers());        
        ClearServers();
        for (int i = 0; i < serverCount; i++)
        {
            string[] temp;
            temp = serverList[i].Split('&');
            Server newServer = new Server(temp);
            servers.Add(newServer.serverName, newServer);
        }        
        GameObject tempObject = null;

        foreach (Server server in servers.Values)
        {
            tempObject = Instantiate(serverRowPrefab, this.transform.position, Quaternion.identity) as GameObject;
            tempObject.transform.SetParent(transform.parent.Find("Scroll View/Viewport/Content"));
            tempObject.transform.localScale = Vector3.one;
            tempObject.transform.Find("ServerName").GetComponent<UnityEngine.UI.Text>().text = server.serverName;
            tempObject.transform.Find("Players").GetComponent<UnityEngine.UI.Text>().text = server.playerCount + "/4";
            if (server.security)
            {
                tempObject.transform.Find("Security").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("lock");
            }

            toDelete.Add(tempObject);

        }
    }

    private void splitString(string message)
    {
        string temp;
        temp = message.Substring(6, message.Length - 6);
        serverList = temp.Split('#');
        serverCount = serverList.Length;
    }

    private void ClearServers()
    {
        servers.Clear();
        for (int i = 0; i < toDelete.Count; i++)
        {            
            Destroy(toDelete[i]);
        }            
        toDelete.Clear();        
    }

    public string GetServers()
    {
        string url = "http://apedestrian.com/bombertale/GetServers.php?unityPassword=" + unityPassword;
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

        // check for errors
        if (!string.IsNullOrEmpty(www.error))
            Debug.Log("WWW Error: " + www.error);
    }


}
