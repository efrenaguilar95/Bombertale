<?php

	include 'Setup.php';

	if(!(
		isset($_GET['serverName']) || 
		isset($_GET['players'])
		))
	{
		include 'Cleanup.php';
		die('BD03: Missing arguments');
	}

	$serverName = strip_tags($_GET['serverName']);
	$players = strip_tags($_GET['players']);

	$query = "UPDATE $serversTablename SET players='$players' WHERE server_name = '$serverName'";

	echo mysql_query($query) === TRUE ? 'BL09: Updated players successfully' : 'BL10: Failed to update players';

	include 'Cleanup.php';

?>