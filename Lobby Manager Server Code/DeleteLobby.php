<?php
	include 'setup.php';


	$gameName = mysql_real_escape_string($_GET['gameName']);
	
	$query = "DELETE FROM $tablename WHERE game_name = '$gameName'";

	echo mysql_query($query) === TRUE ? 'BL05' : 'BL06';

	include 'cleanup.php';
?>