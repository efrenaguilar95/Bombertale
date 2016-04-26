<?php
	include 'setup.php';


	//assumed all info is set, I know this is dangerous =/
	$clientUsername = $_GET['clientUsername'];
	$clientPassword = sha1($_GET['clientPassword']);
	$clientEmail = $_GET['clientEmail'];

	$query = "INSERT INTO $usertablename (username, password, online)
				VALUES ('$clientUsername', '$clientPassword', '$clientEmail', '0')";

	echo mysql_query($query) === TRUE ? 'BC01: Account created' : 'BC00: Username taken';

	include 'cleanup.php';
?>