<?php
	$servername = 		"localhost";
	$databasename = 	"Bombertale";
	$usersTablename = 	"Clients";
	$username = 		"someUser";
	$password = 		"wSiphnsu6gco";


	$conn = mysql_connect($servername, $username, $password);

	if (!$conn)
		die('<span style="color: red">Failed to update password</span>');

	if(!mysql_select_db($databasename))
	{
		mysql_close($conn);
		die('<span style="color: red">Failed to update password</span>');
	}

	if(!(
			isset($_GET['resetId']) ||
			isset($_GET['newPassword'])
		))
	{
		include 'Cleanup.php';
		die('<span style="color: red">Failed to update password</span>');
	}

	$resetId = strip_tags($_GET['resetId']);

	$idLength = strlen($resetId);
	$idLength -= 40;
	$id = substr($resetId, $idLength*-1, $idLength);

	$query = "SELECT * FROM $usersTablename WHERE id = '$id' LIMIT 1";
	$result = mysql_query($query);

	$rows = mysql_num_rows($result);

	if($rows == 0)
	{
		include 'Cleanup.php';
		die('<span style="color: red">Failed to update password</span>');
	}

	$resetIdFromDatabase = sha1(mysql_result($result, 0, 'password'));
	$resetIdFromDatabase .= $id;

	if($resetId != $resetIdFromDatabase)
	{
		include 'Cleanup.php';
		die('<span style="color: red">Failed to update password</span>');
	}

	$newPassword = sha1(strip_tags($_GET['newPassword']) . "theDOG");

	$query = "UPDATE $usersTablename SET password = '$newPassword' WHERE id = '$id'";

	echo mysql_query($query) === TRUE ? '<span style="color: green">Password updated!</span>' : '<span style="color: red">Failed to update password, try resending reset request.</span>';

	include 'Cleanup.php';
?>