<?php

	include 'Setup.php';

	if(!(
			isset($_GET['clientUsername']) ||
			isset($_GET['clientPassword']) ||
			isset($_GET['clientEmail'])
		))
	{
		include 'Cleanup.php';
		die('BD03: Missing arguments');
	}

	$clientUsername = strip_tags($_GET['clientUsername']);
	$clientPassword = sha1(strip_tags($_GET['clientPassword']));
	$clientEmail = strip_tags($_GET['clientEmail']);

	//check to see if email is already in use
	$query = "SELECT * FROM $usersTablename WHERE email = '$clientEmail'";
	$result = mysql_query($query);
	
	if(!$result)
	{
		include 'Cleanup.php';
		die('BD04: ' . mysql_error());
	}

	$rows = mysql_num_rows($result);

	if($rows > 0)
	{
		include 'Cleanup.php';
		die('BC03: Email already in use');
	}

	//check to see if username is already in use
	$query = "SELECT * FROM $usersTablename WHERE username = '$clientUsername'";
	$result = mysql_query($query);
	if(!$result)
	{
		include 'Cleanup.php';
		die('BD04: ' . mysql_error());
	}

	$rows = mysql_num_rows($result);

	if($rows > 0)
	{
		include 'Cleanup.php';
		die('BC02: Username taken');
	}


	$query = "INSERT INTO $usersTablename (username, password, email, online)
				VALUES ('$clientUsername', '$clientPassword', '$clientEmail', '0')";

	echo mysql_query($query) === TRUE ? 'BC01: Account created' : 'BC06: Failed to create account';

	include 'Cleanup.php';

?>