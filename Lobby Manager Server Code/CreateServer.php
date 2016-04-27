<?php

	include 'Setup.php';

	if(!(
			isset($_GET['serverName']) ||
			isset($_GET['serverIp']) ||
			isset($_GET['port']) ||
			isset($_GET['private']) ||
			isset($_GET['password']) ||
			isset($_GET['players'])
		))
	{
		include 'Cleanup.php';
		die('BD03: Missing arguments');
	}

	$serverName = 	strip_tags($_GET['serverName']);
	$serverIp = 	strip_tags($_GET['serverIp']);
	$port = 		strip_tags($_GET['port']);
	$private = 		strip_tags($_GET['private']);
	$password = 	strip_tags($_GET['password']);
	$players = 		strip_tags($_GET['players']);

	$query = "SELECT * FROM $serversTablename WHERE server_name = '$serverName'";

	//if(!$result)
	//{
	//	include 'Cleanup.php';
	//	die('BD04: ' . mysql_error());
	//}

	$rows = mysql_num_rows($result);

	if($rows > 0)
	{
		include 'Cleanup.php';
		die('BL04: Server name already taken');
	}


	$query = "INSERT INTO $serversTablename (server_name, server_ip, port, private, password, players)
				VALUES ('$serverName', '$serverIp', '$port', '$private', '$password', '$players')";

	echo mysql_query($query) === TRUE ? 'BL03: Server created successfully' : 'BL11: Failed to create server';

	include 'Cleanup.php';

?>