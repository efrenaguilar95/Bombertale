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
	$timestamp = 	date('Y-m-d G:i:s');


	$query = "INSERT INTO $serversTablename (server_name, server_ip, port, private, password, players, last_checkin)
				VALUES ('$serverName', '$serverIp', '$port', '$private', '$password', '$players', '$timestamp')";

	echo mysql_query($query) === TRUE ? 'BL03: Server created successfully' : 'BL11: Failed to create server';

	include 'Cleanup.php';

?>