using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DatabaseManager : MonoBehaviour
{

	public WWW www;
	public bool isDone = false;
	private string unityPassword = "ICS168";

    void OnApplicationQuit()
    {
        this.Logout(UIManager.userName);
    }
	/*
		RETURN CODES
		All these functions can return:
			BD00:	Missing or incorrect unity access password
			BD01:	Failed to connect to server
			BD02:	Failed to select database
			BD03:	Missing arguments
			BD04:	Unknown database error
	*/


	public void GetServers()
	{
		/*
			CAN RETURN:
				BL00:	Got servers successfully
				BL01:	No servers hosted
		*/
		string url = "http://apedestrian.com/bombertale/GetServers.php?unityPassword=" + unityPassword;
		SendRequest (url);
	}

	public void DeleteServer(string serverName)
	{
		/*
			CAN RETURN:
				BL05:	Server deleted successfully
				BL06:	Failed to delete server
		*/
		string url = "http://apedestrian.com/bombertale/DeleteServer.php?unityPassword=" + unityPassword + "&serverName=" + serverName;
		SendRequest (url);
	}

	public void CreateServer(string serverName, string ip, int port, bool isPrivate, string password, int players)
	{
		/*
			CAN RETURN:
				BL03:	Server created successfully
				BL11:	Failed to create server
		*/
		string isPrivateString = isPrivate ? "1" : "0";

		string url = "http://apedestrian.com/bombertale/CreateServer.php?unityPassword=" + unityPassword + "&serverName=" + serverName + "&serverIp=" + ip + "&port=" + port + "&private=" + isPrivateString + "&password=" + password + "&players=" + players;
		SendRequest (url);
	}

	public void JoinServer(string serverName)
	{
		/*
			CAN RETURN:
				BL08:	Could not find server by that name
				BL12:	Server is full
				BL13:	Server is ready to be joined
		*/
		string url = "http://apedestrian.com/bombertale/JoinServer.php?unityPassword=" + unityPassword +  "&serverName=" + serverName;
		SendRequest (url);
	}

	public void UpdatePlayers(string serverName, int players)
	{
		/*
			CAN RETURN:
				BL09:	Updated players successfully
				BL10:	Failed to update players
		*/
		string url = "http://apedestrian.com/bombertale/UpdatePlayers.php?unityPassword=" + unityPassword +  "&serverName=" + serverName + "&players=" + players;
		SendRequest (url);
	}

	public void CreateAccount(string username, string password, string email)
	{
		/*
			CAN RETURN:
				BC01:	Account created
				BC02:	Username taken
				BC03:	Email already in use
				BC06:	Failed to create account
		*/
		string url = "http://apedestrian.com/bombertale/CreateAccount.php?unityPassword=" + unityPassword +  "&clientUsername=" + username + "&clientPassword=" + password + "&clientEmail=" + email;
		SendRequest (url);
	}

	public void Login(string username, string password)
	{
        /*
			CAN RETURN:
				BC00:	Login successful
				BC04:	Failed to find account by that name
				BC05:	Incorrect password
				BC10:	User already logged in
				BC11:	Failed to login
		*/
        if (username == "")
            return;
		string url = "http://apedestrian.com/bombertale/Login.php?unityPassword=" + unityPassword +  "&clientUsername=" + username + "&clientPassword=" + password;
		SendRequest (url);
	}

	public void SendLoginResetEmail(string email)
	{
		/*
			CAN RETURN:
				BC07:	Reset email sent
				BC08:	No such email in database
				BC09:	Failed to send email
		*/
		string url = "http://apedestrian.com/bombertale/SendLoginResetEmail.php?unityPassword=" + unityPassword +  "&clientEmail=" + email;
		SendRequest (url);
	}

	public void Logout(string clientUsername)
	{
		/*
			CAN RETURN:
				BC12:	Logout successful
				BC13:	Failed to logout
		*/
		string url = "http://apedestrian.com/bombertale/Logout.php?unityPassword=" + unityPassword +  "&clientUsername=" + clientUsername;
		SendRequest (url);
	}

	public void ServerCheckin(string serverName)
	{
		/*
			CAN RETURN:
				BL14:	Checkin successful
				BL15:	Failed to checkin
		*/
		string url = "http://apedestrian.com/bombertale/ServerCheckin.php?unityPassword=" + unityPassword +  "&serverName=" + serverName;
		SendRequest (url);
	}

	public void clientCheckin(string clientUsername)
	{
		/*
			CAN RETURN:
				BC14:	Checkin successful
				BC15:	Failed to checkin
		*/
		string url = "http://apedestrian.com/bombertale/ServerCheckin.php?unityPassword=" + unityPassword +  "&clientUsername=" + clientUsername;
		SendRequest (url);
	}

	public void cannotConnect(string serverName)
	{
		/*
			CAN RETURN:
				BL08:	Could not find server by that name
				BL16:	Reported dead server
				BL17:	Failed to report dead server
				BL18:	Server has checked in recently, did not report
		*/
		string url = "http://apedestrian.com/bombertale/CannotConnect.php?unityPassword=" + unityPassword +  "&serverName=" + serverName;
		SendRequest (url);
	}

	public string getWWWText()
	{
		if (www == null)
			return "waiting";
		//Debug.Log (www);
		if (www.isDone && !isDone)
		{
			isDone = www.isDone;
			return www.text;
		}
		return "waiting";
	}

	private void SendRequest(string url)
	{
		www = null;
		www = new WWW(url.Replace(" ", "%20"));
		isDone = false;
		StartCoroutine(WaitForRequest(www));
		//while(!www.isDone)
		//{
			//...
		//}
		//return www.text;
	}

	///*
	private IEnumerator WaitForRequest(WWW www)
	{
		//while(!www.isDone)
		//{
			yield return www;

			// check for errors
			if(!string.IsNullOrEmpty(www.error))
				Debug.Log("WWW Error: "+ www.error);
			else
				Debug.Log("WWW success: "+ www.text);
		//}
	}
	//*/
}
