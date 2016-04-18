<?php
	include 'setup.php';


	//assumed all info is set, I know this is dangerous =/
	$clientUsername = $_GET['clientUsername'];

	$query = "SELECT * FROM $usertablename WHERE username = '$clientUsername' LIMIT 1";

	$result = mysql_query($query);

	if(!$result)
	{
		include 'cleanup.php';
		die('BC02: Could not find account with that name');
	}

	$rows = mysql_num_rows($result);

	if($rows == 0)
	{
		include 'cleanup.php';
		die('BC02: Could not find account with that name');
	}

	$encodedPassword = mysql_result($result, 0, 'password');

	if($encodedPassword != sha1($_GET['clientPassword']))
	{
		include 'cleanup.php';
		die('BC03: Incorrect password');
	}

	echo 'BC04: Connected successfully!';

	include 'cleanup.php';
?>