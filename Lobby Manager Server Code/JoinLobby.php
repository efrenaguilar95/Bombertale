<?php
	include 'setup.php';


	//assumed all info is set, I know this is dangerous =/
	$serverName = mysql_real_escape_string($_GET['serverName']);
	
	$query = "SELECT * FROM $tablename WHERE server_name = '$serverName' LIMIT 1";

	$result = mysql_query($query);

	if(!$result)
	{
		include 'cleanup.php';
		die('BL09');
	}


	$echoString = '';

	$rows = mysql_num_rows($result);

	if($rows == 0)
	{
		include 'cleanup.php';
		die('BL09');
	}

	for($i = 0; $i < $rows; ++$i)
	{
		$echoString .= mysql_result($result, $i, 'server_name') . '&';
		$echoString .= mysql_result($result, $i, 'server_ip')   . '&';
		$echoString .= mysql_result($result, $i, 'port')  . '&';
		$echoString .= mysql_result($result, $i, 'players')   . '#';
	}
	$echoString = rtrim($echoString, '#');
	echo $echoString;

	include 'cleanup.php';
?>