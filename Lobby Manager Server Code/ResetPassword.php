<?php

	$servername = 		"localhost";
	$databasename = 	"Bombertale";
	$usersTablename = 	"Clients";
	$username = 		"someUser";
	$password = 		"wSiphnsu6gco";

	$conn = mysql_connect($servername, $username, $password);

	if (!$conn)
		die('Failed to connect to server');

	if(!mysql_select_db($databasename))
	{
		mysql_close($conn);
		die('Failed to select database');
	}

	if(!(
			isset($_GET['resetId'])
		))
	{
		include 'Cleanup.php';
		die('No reset ID');
	}

	$resetId = strip_tags($_GET['resetId']);

	$idLength = strlen($resetId);
	$id = substr($resetId, -($idLength), $idLength);

	$query = "SELECT * FROM $usersTablename WHERE id = '$id' LIMIT 1";
	$result = mysql_query($query);

	$rows = mysql_num_rows($result);

	if($rows == 0)
	{
		include 'Cleanup.php';
		die('Expired reset id');
	}

	$username = mysql_result($result, 0, 'username');
	$resetIdFromDatabase = sha1($resetId = sha1(mysql_result($result, 0, 'password')););
	$resetIdFromDatabase[ .= $id;

	if($resetId != $resetIdFromDatabase)
	{
		include 'Cleanup.php';
		die('Expired reset id');
	}

	echo "its a match!";


	include 'Cleanup.php';
?>