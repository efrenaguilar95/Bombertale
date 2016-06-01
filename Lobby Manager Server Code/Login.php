<?php

	include 'Setup.php';

	if(!(
			isset($_GET['clientUsername']) ||
			isset($_GET['clientPassword'])
		))
	{
		include 'Cleanup.php';
		die('BD03: Missing arguments');
	}

	$clientUsername = strip_tags($_GET['clientUsername']);
	$clientPassword = sha1(strip_tags($_GET['clientPassword'] . "theDOG"));

	$query = "SELECT * FROM $usersTablename WHERE username = '$clientUsername' LIMIT 1";
	$result = mysql_query($query);

	if(!$result)
	{
		include 'Cleanup.php';
		die('BD04: ' . mysql_error());
	}

	$rows = mysql_num_rows($result);

	if($rows == 0)
	{
		include 'Cleanup.php';
		die('BC04: Failed to find account by that name');
	}



	if(mysql_result($result, 0, 'online') == '1')
	{
		$lastCheckin = strtotime(mysql_result($result, 0, 'last_checkin'));

		//if it has been more than 5mins since the user checked in, they probably disconnected
		if(time() - $lastCheckin < 300)
		{
			include 'Cleanup.php';
			die('BC10: User already logged in');
		}
	}

	$encodedPassword = mysql_result($result, 0, 'password');

	if($encodedPassword != $clientPassword)
	{
		include 'Cleanup.php';
		die('BC05: Incorrect password');
	}

	$timestamp = date('Y-m-d G:i:s');

	$query = "UPDATE $usersTablename SET online=1, last_checkin = '$timestamp' WHERE username = '$clientUsername'";

	echo mysql_query($query) === TRUE ? 'BC00: Login successful' : 'BC11: Failed to login';

	include 'Cleanup.php';

?>