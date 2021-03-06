﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ServerCreation : MonoBehaviour {
    public InputField nameInput, ipInput, portInput, passwordInput;

    void Awake()
    {
        nameInput.placeholder.GetComponent<Text>().text = "Bombertale";
        ipInput.placeholder.GetComponent<Text>().text = Network.player.ipAddress;
        portInput.placeholder.GetComponent<Text>().text = "9001";
    }

    public void CreateServer()
    {
        if (nameInput.text == "")
            NetworkHost.ServerName = nameInput.placeholder.GetComponent<Text>().text;
        else
            NetworkHost.ServerName = nameInput.text;

        if (ipInput.text == "")
            NetworkHost.ServerIP = ipInput.placeholder.GetComponent<Text>().text;
        else
            NetworkHost.ServerIP = ipInput.text;

        if (portInput.text == "")
            NetworkHost.Port = int.Parse(portInput.placeholder.GetComponent<Text>().text);
        else
            NetworkHost.Port = int.Parse(portInput.text);

        if (passwordInput.text == "")
        {
            NetworkHost.IsPrivate = false;
            NetworkHost.ServerPassword = "";
        }
        else
        {
            NetworkHost.IsPrivate = true;
            NetworkHost.ServerPassword = passwordInput.text;
        }

        SceneManager.LoadScene("ServerLobby");
    }
}
