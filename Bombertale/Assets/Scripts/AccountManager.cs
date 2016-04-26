using UnityEngine;
using System.Collections;

public class AccountManager : MonoBehaviour {

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
	}
}
