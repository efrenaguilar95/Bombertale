<?php

	include 'Setup.php';

	$query = "SELECT * FROM  $serversTablename";

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
		die('BL01: No servers hosted');
	}

	$echoString = 'BL00: ';

	for($i = 0; $i < $rows; ++$i)
	{
		$echoString .= mysql_result($result, $i, 'server_name') . '&';
		$echoString .= mysql_result($result, $i, 'server_ip') . '&';
		$echoString .= mysql_result($result, $i, 'port') . '&';
		$echoString .= mysql_result($result, $i, 'private')   . '&';
		$echoString .= mysql_result($result, $i, 'password')  . '&';
		$echoString .= mysql_result($result, $i, 'players')   . '#';
	}

	$echoString = rtrim($echoString, '#');
	
	echo $echoString;

	include 'Cleanup.php';

?>