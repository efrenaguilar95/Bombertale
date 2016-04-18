<?php
	include 'setup.php';


	//assumed all info is set, I know this is dangerous =/
	$serverName = $_GET['serverName'];
	
	$query = "SELECT * FROM $tablename WHERE server_name = '$serverName' LIMIT 1";

	$result = mysql_query($query);

	if(!$result)
	{
		include 'cleanup.php';
		die('BL09: Could not find server by that name');
	}

	$rows = mysql_num_rows($result);

	if($rows == 0)
	{
		include 'cleanup.php';
		die('BL09: Could not find server by that name');
	}

	$players = mysql_result($result, 0, 'players');

	if((int)$players >= 4)
	{
		include 'cleanup.php';
		die('BL10: Server is full');
	}

	$echoString  = mysql_result($result, 0, 'server_name') . '&';
	$echoString .= mysql_result($result, 0, 'server_ip')   . '&';
	$echoString .= mysql_result($result, 0, 'port')		   . '&';
	$echoString .= $players;

	echo $echoString;

	include 'cleanup.php';
?>