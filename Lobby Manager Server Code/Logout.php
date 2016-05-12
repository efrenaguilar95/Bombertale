<?php

	include 'Setup.php';

	if(!(
		isset($_GET['username'])
		))
	{
		include 'Cleanup.php';
		die('BD03: Missing arguments');
	}

	$clientUsername = strip_tags($_GET['clientUsername']);

	$query = "UPDATE $usersTablename SET online='0' WHERE username = '$clientUsername'";

	echo mysql_query($query) === TRUE ? 'BL12: Logged out successfully' : 'BL13: Failed to logout';

	include 'Cleanup.php';

?>