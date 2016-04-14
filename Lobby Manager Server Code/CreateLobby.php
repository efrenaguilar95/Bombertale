<?php
	include 'setup.php';

	//assumed all info is set, I know this is dangerous =/
	$serverName = mysql_real_escape_string($_GET['serverName']);
	$serverIp = $_GET['serverIp'];

	$port = $_GET['port'];
	$private = $_GET['private'];
	$password = mysql_real_escape_string($_GET['password']);
	$players = $_GET['players'];
	$query = "INSERT INTO $tablename (server_name, server_ip, port, private, password, players)
				VALUES ('$serverName', '$serverIp', '$port', '$private', '$password', '$players')";

	echo mysql_query($query) === TRUE ? 'BL03' : 'BL04';

	include 'cleanup.php';
?>