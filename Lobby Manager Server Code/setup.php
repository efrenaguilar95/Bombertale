<?php
	$servername = "localhost";
	$databasename = "Bombertale";
	$tablename = "Servers";
	$usertablename = "Clients";
	$username = "someUser";
	$password = "wSiphnsu6gco";
	$unityPassword = "ICS168";

	if(!(isset($_GET['unityPassword']) && $_GET['unityPassword'] == $unityPassword))
		die('BL00: Missing or incorrect password');
	

	$conn = mysql_connect($servername, $username, $password);

	if (!$conn)
		die('BL01: Failed to connect to server');

	if(!mysql_select_db($databasename))
	{
		mysql_close($conn);
		die('BL02: Failed to select database'); //failed to select table
	}
?>