<?php
	include 'setup.php';

	//assumed all info is set, I know this is dangerous =/
	$gameName = mysql_real_escape_string($_GET['gameName']);
	$hostIp = $_GET['hostIp'];
	$port = $_GET['port'];
	$private = $_GET['private'];
	$password = mysql_real_escape_string($_GET['password']);
	$players = $_GET['players'];
	$query = "INSERT INTO $tablename (game_name, host_ip, port, private, password, players)
				VALUES ('$gameName', '$hostIp', '$port', '$private', '$password', '$players')";

	echo mysql_query($query) === TRUE ? 'BL03' : 'BL04';

	include 'cleanup.php';
?>