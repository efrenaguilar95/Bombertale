using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	private string unityPassword = "ICS168";
    private DatabaseManager _databaseManager;
    public static string userName;

    void Awake()
    {
        _databaseManager = this.GetComponent<DatabaseManager>();
    }

	void Update()
	{
		string response = _databaseManager.getWWWText ();
		if (response!="waiting") 
		{
			string returnCode = response.Substring (0, 4);
			if (returnCode == "BC00") { //Login successful
				string username = GameObject.Find ("Canvas/Username").GetComponent<UnityEngine.UI.InputField> ().text;
				userName = username;
				GameObject.Find ("Canvas/ErrorText").GetComponent<UnityEngine.UI.Text> ().text = "";
				SceneManager.LoadScene ("JoinOrHost");
			} else if (returnCode == "BC04" || returnCode == "BC05") {
				//wrong login information
				GameObject.Find ("Canvas/ErrorText").GetComponent<UnityEngine.UI.Text> ().text = "Wrong Login Information";
			} else if (returnCode == "BC10") {
				//user logged in
				GameObject.Find ("Canvas/ErrorText").GetComponent<UnityEngine.UI.Text> ().text = "That User Is Already Logged In";
			} else if (returnCode == "BC01") {
				//account created!
				GameObject.Find ("Canvas/ErrorText").GetComponent<UnityEngine.UI.Text> ().text = "";
				SceneManager.LoadScene ("Login");
			} else if (returnCode == "BC02") {
				//username taken
				GameObject.Find ("Canvas/ErrorText").GetComponent<UnityEngine.UI.Text> ().text = "Username Taken";
			} else if (returnCode == "BC03") {
				//email in use
				GameObject.Find ("Canvas/ErrorText").GetComponent<UnityEngine.UI.Text> ().text = "Email Already In Use";
			} 
			else if (returnCode == "BC07") 
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
	}

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
        _databaseManager.Logout(UIManager.userName);
        //Debug.Log(_databaseManager.Logout(UIManager.userName));
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
        _databaseManager.Logout(UIManager.userName);
        Application.Quit();
    }
	public void Login()
	{
		string username = GameObject.Find("Canvas/Username").GetComponent<UnityEngine.UI.InputField>().text;
		string password = GameObject.Find("Canvas/Password").GetComponent<UnityEngine.UI.InputField>().text;

		_databaseManager.Login(username, password);

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

        _databaseManager.CreateAccount(username, password, email);
	}

	public void ForgotLogin()
	{
		SceneManager.LoadScene("ForgotLogin");
	}

	public void SubmitForgotLogin()
	{
		string email = GameObject.Find("Canvas/Email").GetComponent<UnityEngine.UI.InputField>().text;
        _databaseManager.SendLoginResetEmail(email);
	}



}
