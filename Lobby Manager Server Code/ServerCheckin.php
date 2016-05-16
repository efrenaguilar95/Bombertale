<?php

	include 'Setup.php';

	if(!(
		isset($_GET['serverName'])
		))
	{
		include 'Cleanup.php';
		die('BD03: Missing arguments');
	}

	$serverName = strip_tags($_GET['serverName']);

	$timestamp = date('Y-m-d G:i:s');

	$query = "UPDATE $serversTablename SET last_checkin = '$timestamp' WHERE server_name = '$serverName'";

	echo mysql_query($query) === TRUE ? 'BL15: Checked in server successfully' : 'BL14: Failed to checkin server';

	include 'Cleanup.php';

?>