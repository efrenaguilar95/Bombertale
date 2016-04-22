using UnityEngine;
using System.Collections;

public class LobbyManager : MonoBehaviour
{
	private string unityPassword = "ICS168";

	//void Start()
	//{
	//	//DeleteServer ("alex");
	//	//CreateServer("alex2", "1.1.1.1", 100, true, "sonic", 4);
	//	//GetServers();
	//	//JoinServer("alex");
	//	//UpdatePlayers("alex", 2);
	//	//CreateAccount("alex", "sonic");
	//	//Login("alex", "sonic");

	//}

	//returns the name, whether it is private, password (empty string if public game), and players for all lobbies
	public string GetServers()
	{
		string url = "http://apedestrian.com/bombertale/GetLobby.php?unityPassword=" + unityPassword;
		return GetText (url);
	}

	//assumed that only letters, numbers, and spaces are used in game name
	public string DeleteServer(string serverName)
	{
		string url = "http://apedestrian.com/bombertale/DeleteLobby.php?unityPassword=" + unityPassword + "&serverName=" + serverName;
		return GetText (url);
	}

	//assumed that only letters, numbers, and spaces are used in game name and password.
	//assumed IP is in format xxx.xxx.xxx.xxx
	public string CreateServer(string serverName, string ip, int port, bool isPrivate, string password, int players)
	{
		string isPrivateString = isPrivate ? "1" : "0";

		string url = "http://apedestrian.com/bombertale/CreateLobby.php?unityPassword=" + unityPassword + "&serverName=" + serverName + "&serverIp=" + ip + "&port=" + port + "&private=" + isPrivateString + "&password=" + password + "&players=" + players;
		return GetText (url);
	}

	public string JoinServer(string serverName)
	{
		string url = "http://apedestrian.com/bombertale/JoinLobby.php?unityPassword=" + unityPassword +  "&serverName=" + serverName;
		return GetText (url);
	}

	public string UpdatePlayers(string serverName, int players)
	{
		string url = "http://apedestrian.com/bombertale/UpdatePlayers.php?unityPassword=" + unityPassword +  "&serverName=" + serverName + "&players=" + players;
		return GetText (url);
	}

	public string CreateAccount(string username, string password)
	{
		string url = "http://apedestrian.com/bombertale/CreateAccount.php?unityPassword=" + unityPassword +  "&clientUsername=" + username + "&clientPassword=" + password;
		return GetText (url);
	}

	public string Login(string username, string password)
	{
		string url = "http://apedestrian.com/bombertale/Login.php?unityPassword=" + unityPassword +  "&clientUsername=" + username + "&clientPassword=" + password;
		return GetText (url);
	}

	private string GetText(string url)
	{
		WWW www = new WWW(url.Replace(" ", "%20"));
		StartCoroutine(WaitForRequest(www));
		while(!www.isDone)
		{
			//...
		}
		return www.text;
	}

	private IEnumerator WaitForRequest(WWW www)
	{
		yield return www;

		// check for errors
		if(!string.IsNullOrEmpty(www.error))
			Debug.Log("WWW Error: "+ www.error);
		else
			Debug.Log("WWW Success!: "+ www.text);   
	}
}
