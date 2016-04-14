<?php
	include 'db_info.php';

	//write 0 to the html page if the password is missing or incorrect
	if(!(isset($_GET['unityPassword']) && $_GET['unityPassword'] == $unityPassword))
		die('password is missing or incorrect');


//write 0 to the page if connecting to the datbase or selecting the table fails
	$conn = mysql_connect($servername, $username, $password);
	if (!$conn) die(mysql_error());
	mysql_select_db($databasename) or die(mysql_error());


	//assumed all info is set, I know this is dangerous =/
	$gameName = mysql_real_escape_string($_GET['gameName']);
	$hostIp = $_GET['hostIp'];
	$port = $_GET['port'];
	$private = $_GET['private'];
	$password = mysql_real_escape_string($_GET['password']);
	$players = $_GET['players'];
	$query = "INSERT INTO $tablename (game_name, host_ip, port, private, password, players)
				VALUES ('$gameName', '$hostIp', '$port', '$private', '$password', '$players')";

	echo mysql_query($query) === TRUE ? '1' : mysql_error();
?>