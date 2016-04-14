<?php
	include 'db_info.php';

	
	if(!(isset($_GET['unityPassword']) && $_GET['unityPassword'] == $unityPassword))
		die('BL00');



	$conn = mysql_connect($servername, $username, $password);
	if (!$conn) die('BL01');
	mysql_select_db($databasename) or die('BL02');


	//assumed all info is set, I know this is dangerous =/
	$gameName = mysql_real_escape_string($_GET['gameName']);
	
	$query = "DELETE FROM $tablename WHERE game_name = '$gameName'";

	echo mysql_query($query) === TRUE ? 'BL05' : 'BL06';
?>