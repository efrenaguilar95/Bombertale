<?php
	$servername = 		"localhost";
	$databasename = 	"Bombertale";
	$serversTablename = "Servers";
	$username = 		"someUser";
	$password = 		"wSiphnsu6gco";


	$conn = mysql_connect($servername, $username, $password);

	if (!$conn)
		die('<span style="color:red"><p>No servers hosted</p></span>');

	if(!mysql_select_db($databasename))
	{
		mysql_close($conn);
		die('<span style="color:red"><p>No servers hosted</p></span>');
	}

	
	$query = "SELECT * FROM  $serversTablename";
	
	$result = mysql_query($query);

	if(!$result)
	{
		include 'Cleanup.php';
		die('<span style="color:red"><p>No servers hosted</p></span>');
	}


	$rows = mysql_num_rows($result);

	if($rows == 0)
	{
		include 'Cleanup.php';
		die('<span style="color:red"><p>No servers hosted</p></span>');
	}

	echo '<ul>';
	for($i = 0; $i < $rows; ++$i)
	{
		echo '<li><a href="http://apedestrian.com/bombertale/watch.html?serverName=' . mysql_result($result, $i, 'server_name') .'"><span style = "color:green">'	. mysql_result($result, $i, 'server_name')	. '</span></a></li>';
	}
	echo '</ul>';

	
	include 'Cleanup.php';
?>