<?php
	include 'setup.php';


	$serverName = mysql_real_escape_string($_GET['serverName']);
	
	$query = "DELETE FROM $tablename WHERE server_name = '$serverName'";

	echo mysql_query($query) === TRUE ? 'BL05' : 'BL06';

	include 'cleanup.php';
?>