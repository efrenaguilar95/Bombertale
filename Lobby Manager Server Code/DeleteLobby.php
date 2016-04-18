<?php
	include 'setup.php';


	$serverName = $_GET['serverName'];
	
	$query = "DELETE FROM $tablename WHERE server_name = '$serverName'";

	echo mysql_query($query) === TRUE ? 'BL05: Server successfully deleted' : 'BL06: Failed to delete server';

	include 'cleanup.php';
?>