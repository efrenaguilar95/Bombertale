<?php
	include 'setup.php';

	$query = "SELECT * FROM  $tablename";
	$result = mysql_query($query);

	if(!$result)
	{
		include 'cleanup.php';
		die('BL07: Failed to get servers'); //failed to get lobbies
	}


	$rows = mysql_num_rows($result);

	if($rows == 0)
	{
		include 'cleanup.php';
		die('BL08: No servers hosted');
	}

	$echoString = 'BL13: ';
	for($i = 0; $i < $rows; ++$i)
	{
		$echoString .= mysql_result($result, $i, 'server_name') . '&';
		$echoString .= mysql_result($result, $i, 'private')   . '&';
		$echoString .= mysql_result($result, $i, 'password')  . '&';
		$echoString .= mysql_result($result, $i, 'players')   . '#';
	}
	$echoString = rtrim($echoString, '#');
	echo $echoString;

	include 'cleanup.php';
?>