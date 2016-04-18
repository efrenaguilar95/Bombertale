<?php
	include 'setup.php';

	$serverName = $_GET['serverName'];
	$serverIp = $_GET['serverIp'];
	$port = $_GET['port'];
	$private = $_GET['private'];
	$password = $_GET['password'];
	$players = $_GET['players'];
	$query = "INSERT INTO $tablename (server_name, server_ip, port, private, password, players)
				VALUES ('$serverName', '$serverIp', '$port', '$private', '$password', '$players')";

	echo mysql_query($query) === TRUE ? 'BL03: Server successfully created' : 'BL04: Server name taken';

	include 'cleanup.php';
?>