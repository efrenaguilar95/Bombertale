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
	$clientPassword = sha1(strip_tags($_GET['clientPassword']));

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

	$encodedPassword = mysql_result($result, 0, 'password');

	if($encodedPassword != $clientPassword)
	{
		include 'Cleanup.php';
		die('BC05: Incorrect password');
	}

	echo 'BC00: Login successful';

	include 'Cleanup.php';

?>