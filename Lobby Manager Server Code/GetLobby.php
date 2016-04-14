<?php
	include 'db_info.php';

	//write 0 to the html page if the password is missing or incorrect
	if(!(isset($_GET['unityPassword']) && $_GET['unityPassword'] == $unityPassword))
		die('password is missing or incorrect');


//write 0 to the page if connecting to the datbase or selecting the table fails
	$conn = mysql_connect($servername, $username, $password);
	if (!$conn) die(mysql_error());
	mysql_select_db($databasename) or die(mysql_error());

	$query = "SELECT * FROM  $tablename";
	$result = mysql_query($query);
	if(!$result) die(mysql_error());

	$echoString = '';

	$rows = mysql_num_rows($result);

	for($i = 0; $i < $rows; ++$i)
	{
		$echoString .= mysql_result($result, $i, 'game_name') . '&';
		$echoString .= mysql_result($result, $i, 'host_ip')   . '&';
		$echoString .= mysql_result($result, $i, 'port')      . '&';
		$echoString .= mysql_result($result, $i, 'private')   . '&';
		$echoString .= mysql_result($result, $i, 'password')  . '&';
		$echoString .= mysql_result($result, $i, 'players')   . '#';
	}
	$echoString = rtrim($echoString, '#');
	echo $echoString;
	
?>