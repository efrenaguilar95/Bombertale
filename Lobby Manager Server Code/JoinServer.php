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
	
	$query = "SELECT * FROM $serversTablename WHERE server_name = '$serverName' LIMIT 1";

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
		die('BL08: Could not find server by that name');
	}

	$players = mysql_result($result, 0, 'players');

	if((int)$players >= 4)
	{
		include 'Cleanup.php';
		die('BL12: Server is full');
	}

	$echoString = 'BL13: ';

	$echoString  = mysql_result($result, 0, 'server_name') . '&';
	$echoString .= mysql_result($result, 0, 'server_ip')   . '&';
	$echoString .= mysql_result($result, 0, 'port')		   . '&';
	$echoString .= $players;

	echo $echoString;

	include 'Cleanup.php';
?>