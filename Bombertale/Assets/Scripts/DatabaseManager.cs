﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DatabaseManager : MonoBehaviour
{
	private string unityPassword = "ICS168";

	void Start()
	{
		//DeleteServer ("alex");
		//CreateServer("alex2", "1.1.1.1", 100, true, "sonic", 4);
		//GetServers();
		//JoinServer("Alex's Lobby");
		//UpdatePlayers("Alex's Lobby", 3);
		//CreateAccount("alex", "sonic", "fafke@uci.edu");
		//Login("alex", "sonic");

	}

	//returns the name, whether it is private, password (empty string if public game), and players for all lobbies
	public string GetServers()
	{
		string url = "http://apedestrian.com/bombertale/GetServers.php?unityPassword=" + unityPassword;
		return GetText (url);
	}

	//assumed that only letters, numbers, and spaces are used in game name
	public string DeleteServer(string serverName)
	{
		string url = "http://apedestrian.com/bombertale/DeleteServer.php?unityPassword=" + unityPassword + "&serverName=" + serverName;
		return GetText (url);
	}

	//assumed that only letters, numbers, and spaces are used in game name and password.
	//assumed IP is in format xxx.xxx.xxx.xxx
	public string CreateServer(string serverName, string ip, int port, bool isPrivate, string password, int players)
	{
		string isPrivateString = isPrivate ? "1" : "0";

		string url = "http://apedestrian.com/bombertale/CreateServer.php?unityPassword=" + unityPassword + "&serverName=" + serverName + "&serverIp=" + ip + "&port=" + port + "&private=" + isPrivateString + "&password=" + password + "&players=" + players;
		return GetText (url);
	}

	public string JoinServer(string serverName)
	{
		string url = "http://apedestrian.com/bombertale/JoinServer.php?unityPassword=" + unityPassword +  "&serverName=" + serverName;
		return GetText (url);
	}

	public string UpdatePlayers(string serverName, int players)
	{
		string url = "http://apedestrian.com/bombertale/UpdatePlayers.php?unityPassword=" + unityPassword +  "&serverName=" + serverName + "&players=" + players;
		return GetText (url);
	}

	public string CreateAccount(string username, string password, string email)
	{
		string url = "http://apedestrian.com/bombertale/CreateAccount.php?unityPassword=" + unityPassword +  "&clientUsername=" + username + "&clientPassword=" + password + "&clientEmail=" + email;
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
			Debug.Log("WWW success: "+ www.text);
	}
}