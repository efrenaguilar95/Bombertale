<?php
	$servername = "localhost";
	$databasename = "BombertaleGames";
	$tablename = "HostedGames";
	$username = "Bombertale";
	$password = "wSiphnsu6gco";
	$unityPassword = "ICS168";

	if(!(isset($_GET['unityPassword']) && $_GET['unityPassword'] == $unityPassword))
		die('BL00'); //missing or incorrect unity access password
	

	$conn = mysql_connect($servername, $username, $password);

	if (!$conn)
		die('BL01'); //failed to connect to database

	if(!mysql_select_db($databasename))
	{
		mysql_close($conn);
		die('BL02'); //failed to select table
	}
?>