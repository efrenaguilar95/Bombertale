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
	
	$query = "DELETE FROM $serversTablename WHERE server_name = '$serverName'";

	echo mysql_query($query) === TRUE ? 'BL05: Server deleted successfully' : 'BL06: Failed to delete server';

	include 'Cleanup.php';

?>