using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	private string unityPassword = "ICS168";

    public void PressLocalGame()
    {
        SceneManager.LoadScene("Main");
    }
    public void PressJoinLobby()
    {
        SceneManager.LoadScene("Login");
    } 

	public void BackToMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

    public void Back()
    {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }

	public void Login()
	{
		string username = GameObject.Find("Canvas/Username").GetComponent<UnityEngine.UI.InputField>().text;
		string password = GameObject.Find("Canvas/Password").GetComponent<UnityEngine.UI.InputField>().text;

		string returnCode = Login (username, password);

		returnCode = returnCode.Substring (0, 4);

		if (returnCode == "BC04")
		{
			//login success
			//WE NEED TO SET A GLOBAL USERNAME HERE
			GameObject.Find("Canvas/ErrorText").GetComponent<UnityEngine.UI.Text>().text = "";
			SceneManager.LoadScene ("Lobby");
		}
		else if(returnCode == "BC02" || returnCode == "BC03")
		{
			//wrong login information
			GameObject.Find("Canvas/ErrorText").GetComponent<UnityEngine.UI.Text>().text = "Wrong Login Information";
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
		//string email = GameObject.Find("Canvas/Email").GetComponent<UnityEngine.UI.InputField>().text;

		string returnCode = CreateAccount (username, password);//, email);

		returnCode = returnCode.Substring (0, 4);

		if (returnCode == "BC01")
		{
			//login success
			//WE NEED TO SET A GLOBAL USERNAME HERE
			GameObject.Find("Canvas/ErrorText").GetComponent<UnityEngine.UI.Text>().text = "";
			SceneManager.LoadScene ("Login");
		}
		else if(returnCode == "BC00")
		{
			//username taken
			GameObject.Find("Canvas/ErrorText").GetComponent<UnityEngine.UI.Text>().text = "Username Taken";
		}
		else if (returnCode == "BC05")
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
		//SceneManager.LoadScene("MainMenu");
	}


	public string Login(string username, string password)
	{
		string url = "http://apedestrian.com/bombertale/Login.php?unityPassword=" + unityPassword +  "&clientUsername=" + username + "&clientPassword=" + password;
		return GetText (url);
	}

	public string CreateAccount(string username, string password)//, string email)
	{
		string url = "http://apedestrian.com/bombertale/CreateAccount.php?unityPassword=" + unityPassword +  "&clientUsername=" + username + "&clientPassword=" + password;// + "&clientEmail=" + email;
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
