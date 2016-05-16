using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	private string unityPassword = "ICS168";
    public static string userName;

    public void PressLocalGame()
    {
        SceneManager.LoadScene("Main");
    }
    public void GoToServerList()
    {
        SceneManager.LoadScene("ServerList");
    }

    public void GoToServerLobby()
    {
        SceneManager.LoadScene("ServerLobby");
    }

	public void BackToMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

	public void GoToLogin()
	{
		SceneManager.LoadScene("Login");
	}

    public void GoToTuToriel()
    {
        SceneManager.LoadScene("TuToriel");
    }

    public void goToJoinOrHost()
    {
        SceneManager.LoadScene("JoinOrHost");
    }

    public void Flee()
    {
        Application.Quit();
    }
	public void Login()
	{
		string username = GameObject.Find("Canvas/Username").GetComponent<UnityEngine.UI.InputField>().text;
		string password = GameObject.Find("Canvas/Password").GetComponent<UnityEngine.UI.InputField>().text;

		string returnCode = Login (username, password);

		returnCode = returnCode.Substring (0, 4);

		if (returnCode == "BC00")
		{
            //login success
            userName = username;
			GameObject.Find("Canvas/ErrorText").GetComponent<UnityEngine.UI.Text>().text = "";
			SceneManager.LoadScene ("JoinOrHost");
		}
		else if(returnCode == "BC04" || returnCode == "BC05")
		{
			//wrong login information
			GameObject.Find("Canvas/ErrorText").GetComponent<UnityEngine.UI.Text>().text = "Wrong Login Information";
		}
		else if(returnCode == "BC10")
		{
			//user logged in
			GameObject.Find("Canvas/ErrorText").GetComponent<UnityEngine.UI.Text>().text = "That User Is Already Logged In";
		}
		else
		{
			//could not reach server
			GameObject.Find("Canvas/ErrorText").GetComponent<UnityEngine.UI.Text>().text = "Couldn't Reach Server";
		}
	}

	public void CreateAccount()
	{
		SceneManager.LoadScene("CreateAccount");
	}

	public void CreateNewAccount()
	{
		string username = GameObject.Find("Canvas/Username").GetComponent<UnityEngine.UI.InputField>().text;
		string password = GameObject.Find("Canvas/Password").GetComponent<UnityEngine.UI.InputField>().text;
		string email = GameObject.Find("Canvas/Email").GetComponent<UnityEngine.UI.InputField>().text;

		string returnCode = CreateAccount (username, password, email);

		returnCode = returnCode.Substring (0, 4);

		if (returnCode == "BC01")
		{
			//account created!
			GameObject.Find("Canvas/ErrorText").GetComponent<UnityEngine.UI.Text>().text = "";
			SceneManager.LoadScene ("Login");
		}
		else if(returnCode == "BC02")
		{
			//username taken
			GameObject.Find("Canvas/ErrorText").GetComponent<UnityEngine.UI.Text>().text = "Username Taken";
		}
		else if (returnCode == "BC03")
		{
			//email in use
			GameObject.Find("Canvas/ErrorText").GetComponent<UnityEngine.UI.Text>().text = "Email Already In Use";
		}
		else
		{
			//could not reach server
			GameObject.Find("Canvas/ErrorText").GetComponent<UnityEngine.UI.Text>().text = "Couldn't Reach Server";
		}
	}

	public void ForgotLogin()
	{
		SceneManager.LoadScene("ForgotLogin");
	}

	public void SubmitForgotLogin()
	{
		string email = GameObject.Find("Canvas/Email").GetComponent<UnityEngine.UI.InputField>().text;
		string returnCode = SendLoginResetEmail (email);
		returnCode = returnCode.Substring (0, 4);

		if (returnCode == "BC07")
		{
			//email sent!
			GameObject.Find("Canvas/ErrorText").GetComponent<UnityEngine.UI.Text>().text = "Email Sent!";
		}
		else if(returnCode == "BC08")
		{
			//email not in database
			GameObject.Find("Canvas/ErrorText").GetComponent<UnityEngine.UI.Text>().text = "No Accounts With That Email";
		}
		else if(returnCode == "BC09")
		{
			//email not in database
			GameObject.Find("Canvas/ErrorText").GetComponent<UnityEngine.UI.Text>().text = "Failed To Send Email :(";
		}
		else
		{
			//could not reach server
			GameObject.Find("Canvas/ErrorText").GetComponent<UnityEngine.UI.Text>().text = "Couldn't Reach Server";
		}
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

	public string SendLoginResetEmail(string email)
	{
		string url = "http://apedestrian.com/bombertale/SendLoginResetEmail.php?unityPassword=" + unityPassword +  "&clientEmail=" + email;
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
