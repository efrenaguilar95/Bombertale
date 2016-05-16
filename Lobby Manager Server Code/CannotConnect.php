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

	$lastCheckin = strtotime(mysql_result($result, 0, 'last_checkin'));

	//if it has been less than 5mins since the server checked in, don't delete it
	if(time() - $lastCheckin < 300)
	{
		include 'Cleanup.php';
		die('BL18: Server has checked in recently, did not report');
	}


	$query = "DELETE FROM $serversTablename WHERE server_name = '$serverName'";

	echo mysql_query($query) === TRUE ? 'BL16: Reported dead server' : 'BL17: Failed to report dead server';

	include 'Cleanup.php';

?>