<?php
	include 'db_info.php';

	if(!(isset($_GET['unityPassword']) && $_GET['unityPassword'] == $unityPassword))
		die('BL00');


	$conn = mysql_connect($servername, $username, $password);
	if (!$conn) die('BL01');
	mysql_select_db($databasename) or die('BL02');

	$query = "SELECT * FROM  $tablename";
	$result = mysql_query($query);
	if(!$result) die('BL07');

	$echoString = '';

	$rows = mysql_num_rows($result);

	if($rows == 0)
	{
		die('BL08');
	}

	for($i = 0; $i < $rows; ++$i)
	{
		$echoString .= mysql_result($result, $i, 'game_name') . '&';
		$echoString .= mysql_result($result, $i, 'private')   . '&';
		$echoString .= mysql_result($result, $i, 'password')  . '&';
		$echoString .= mysql_result($result, $i, 'players')   . '#';
	}
	$echoString = rtrim($echoString, '#');
	echo $echoString;
	
?>