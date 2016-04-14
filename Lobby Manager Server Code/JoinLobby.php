<?php
	include 'setup.php';


	//assumed all info is set, I know this is dangerous =/
	$gameName = mysql_real_escape_string($_GET['gameName']);
	
	$query = "SELECT * FROM $tablename WHERE game_name = '$gameName' LIMIT 1";

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
		$echoString .= mysql_result($result, $i, 'game_name') . '&';
		$echoString .= mysql_result($result, $i, 'host_ip')   . '&';
		$echoString .= mysql_result($result, $i, 'port')  . '&';
		$echoString .= mysql_result($result, $i, 'players')   . '#';
	}
	$echoString = rtrim($echoString, '#');
	echo $echoString;

	include 'cleanup.php';
?>