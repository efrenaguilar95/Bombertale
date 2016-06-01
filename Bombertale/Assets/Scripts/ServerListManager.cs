using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ServerListManager : MonoBehaviour
{
    public GameObject serverRowPrefab;
    public Dictionary<string, Server> servers = new Dictionary<string, Server>();
    private DatabaseManager _databaseManager;
    private Text _errorText;
	private GameObject _PWPanel;

    //private List<Server> servers = new List<Server>();
    private string[] serverList;
    private int serverCount;

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
    void Awake()
    {
        _databaseManager = this.GetComponent<DatabaseManager>();
        _errorText = GameObject.Find("Canvas/ErrorText").GetComponent<Text>();
		_PWPanel = GameObject.Find ("Canvas/PWPanel");
		_PWPanel.SetActive (false);
    }
    void Start()
    {
        Refresh();
    }

	void Update()
	{
		string response = _databaseManager.getWWWText ();
		if (response!="waiting") 
		{
			string returnCode = response.Substring(0, 4);
			if (returnCode == "BL00") { //Got servers successfully
				splitString (response);

				for (int i = 0; i < serverCount; ++i) {
					string[] temp;
					temp = serverList [i].Split ('&');
					Server newServer = new Server (temp);
					servers.Add (newServer.serverName, newServer);
				}
				GameObject tempObject = null;

				foreach (Server server in servers.Values) {
					tempObject = Instantiate (serverRowPrefab, this.transform.position, Quaternion.identity) as GameObject;
					tempObject.transform.SetParent (transform.parent.Find ("Scroll View/Viewport/Content"));
					tempObject.transform.localScale = Vector3.one;
					tempObject.transform.Find ("ServerName").GetComponent<UnityEngine.UI.Text> ().text = server.serverName;
					tempObject.transform.Find ("Players").GetComponent<UnityEngine.UI.Text> ().text = server.playerCount + "/4";
					if (server.security) {
						tempObject.transform.Find ("Security").GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> ("lock");
					}

					toDelete.Add (tempObject);

				}
			} 
			else if (returnCode == "BL01") //No servers hosted
			{
				_errorText.text = "No servers hosted";
			}
			else if(returnCode == "BL13") //Server is ready to be joined
			{
				if (NetworkHost.IsPrivate)
				{
					//message box
					_PWPanel.SetActive(true);
					Debug.Log (servers[NetworkHost.ServerName].password);
					string input = GameObject.Find ("Canvas/PWPanel/PasswordInput").GetComponent<UnityEngine.UI.InputField> ().text;
					if(servers[NetworkHost.ServerName].password != input ) //password is WRONG
					{
						Debug.Log ("WHY");
						if (input != "")
						{
							GameObject.Find ("Canvas/PWPanel/PasswordInput").GetComponent<UnityEngine.UI.InputField> ().text = "";
							_PWPanel.SetActive(false);
							_errorText.text = "Wrong Password";
						}
						return;
					}
					else
						SceneManager.LoadScene("ClientLobby");
				}
				else
					SceneManager.LoadScene("ClientLobby");
			}
			else if (returnCode == "BL12")
				_errorText.text = "Server is full";
			else if (returnCode == "BL08")
				_errorText.text = "Server doesn't exist";
			else
				_errorText.text = "Could not connect";
		}
	}

    public void PressJoin()
    {
		_databaseManager.JoinServer (NetworkHost.ServerName);
        _errorText.text = "";

    }

    public void Refresh()
    {
        NetworkHost.ServerIP = "";
        ClearServers();
        _errorText.text = "";


        _databaseManager.GetServers();
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
        serverCount = 0;
        servers.Clear();
        for (int i = 0; i < toDelete.Count; i++)
        {            
            Destroy(toDelete[i]);
        }            
        toDelete.Clear();        
    }
}
