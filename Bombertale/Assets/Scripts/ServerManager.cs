using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ServerManager : MonoBehaviour
{
    public GameObject serverInfo;

    private List<Server> servers = new List<Server>();
    private string[] serverList;
    private int serverCount;
    private string unityPassword = "ICS168";

    List<GameObject> toDelete = new List<GameObject>();

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
        servers.Clear();
        //Debug.Log(transform.parent.Find("Scroll View").Find("Viewport").Find("Content").childCount);
        //if (transform.parent.Find("Scroll View").Find("Viewport").Find("Content").childCount == null)
        //{
        //    return;
        //}
        ClearServers();
        for (int i = 0; i < serverCount; i++)
        {
            string[] temp;
            temp = serverList[i].Split('&');
            servers.Add(new Server(temp));
        }
        //Debug.Log(servers[0].serverName);
        GameObject tempObject = null;

        foreach (Server temp in servers)
        {
            tempObject = Instantiate(serverInfo, this.transform.position, Quaternion.identity) as GameObject;
            tempObject.transform.SetParent(transform.parent.Find("Scroll View/Viewport/Content"));
            tempObject.transform.localScale = Vector3.one;
            tempObject.transform.Find("LobbyName").GetComponent<UnityEngine.UI.Text>().text = temp.serverName;
            tempObject.transform.Find("Players").GetComponent<UnityEngine.UI.Text>().text = temp.playerCount + "/4";
            if (temp.security)
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
        for (int i = 0; i < toDelete.Count; i++)
            Destroy(toDelete[i]);
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
