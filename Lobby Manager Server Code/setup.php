<?php

	$servername = 		"localhost";
	$databasename = 	"Bombertale";
	$serversTablename =	"Servers";
	$usersTablename = 	"Clients";
	$username = 		"someUser";
	$password = 		"wSiphnsu6gco";
	$unityPassword = 	"ICS168";

	if(!(isset($_GET['unityPassword']) && $_GET['unityPassword'] == $unityPassword))
		die('BD00: Missing or incorrect unity access password');

	$conn = mysql_connect($servername, $username, $password);

	if (!$conn)
		die('BD01: Failed to connect to server');

	if(!mysql_select_db($databasename))
	{
		mysql_close($conn);
		die('BD02: Failed to select database');
	}
	
?>