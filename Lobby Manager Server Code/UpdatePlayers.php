<?php
	include 'setup.php';

	//assumed all info is set, I know this is dangerous =/
	$serverName = $_GET['serverName'];
	$players = $_GET['players'];

	$query = "UPDATE $tablename SET players='$players' WHERE server_name = '$serverName'";

	echo mysql_query($query) === TRUE ? 'BL12: Updated players' : 'BL11: Failed to update players';

	include 'cleanup.php';
?>