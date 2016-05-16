<?php

	include 'Setup.php';

	if(!(
		isset($_GET['clientUsername'])
		))
	{
		include 'Cleanup.php';
		die('BD03: Missing arguments');
	}

	$clientUsername = strip_tags($_GET['clientUsername']);

	$timestamp = date('Y-m-d G:i:s');

	$query = "UPDATE $usersTablename SET last_checkin = '$timestamp' WHERE username = '$clientUsername'";

	echo mysql_query($query) === TRUE ? 'BC15: Checked in client successfully' : 'BC14: Failed to checkin client';

	include 'Cleanup.php';

?>