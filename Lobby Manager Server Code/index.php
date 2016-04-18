<?php

	$servername = "localhost";
	$databasename = "Bombertale";
	$usertablename = "Clients";
	$tablename = "Servers";
	$username = "someUser";
	$password = "wSiphnsu6gco";

	$conn = mysql_connect($servername, $username, $password);

	if (!$conn)
		die('BL01: Failed to connect to server '); //failed to connect to database

	if(!mysql_select_db($databasename))
	{
		mysql_close($conn);
		die('BL02: Failed to select database'); //failed to select table
	}


if(isset($_GET['reset']))
{
	$query = "TRUNCATE TABLE $tablename";
	$result = mysql_query($query);
	if(!$result)
	{
		die("Clearing table failed: " . mysql_error());
	}
	header("LOCATION: http://apedestrian.com/bombertale/index.php");
}
	

?>

<!DOCTYPE html>
<html>
<head>
	<style>
		table, th, td
		{
    		border: 1px solid black;
    		border-collapse: collapse;
		}
		th, td
		{
    		padding: 15px;
		}
	</style>
</head>
<body>

<table>
	<tr>
		<th>server_name</th>
		<th>server_ip</th>
		<th>port</th>
		<th>private</th>
		<th>password</th>
		<th>players</th>
	</tr>

<?php
	
	$query = "SELECT * FROM  $tablename";
	$result = mysql_query($query);
	if(!$result)
	{
		die("Database access failed: " . mysql_error());
	}

	$rows = mysql_num_rows($result);

	for($i = 0; $i < $rows; ++$i)
	{
		echo '<tr>';
		echo '<td>'	. mysql_result($result, $i, 'server_name')	. '</td>';
		echo '<td>'	. mysql_result($result, $i, 'server_ip')		. '</td>';
		echo '<td>'	. mysql_result($result, $i, 'port')		. '</td>';
        if(mysql_result($result, $i, 'private') == 0)
        {
        	echo '<td>false</td>';
            echo '<td></td>';
        }
        else
        {
        	echo '<td>true</td>';
         	echo '<td>' . mysql_result($result, $i, 'password') . '</td>';
        }
		echo '<td>'	. mysql_result($result, $i, 'players')		. '/4</td>';
		echo '</tr>';
	}
?>
</table>

<form action="index.php?reset=true" method="get">
	<input type="submit" name="reset" value="Reset">
</form>
<br /><br />
<table>
	<tr>
		<th>username</th>
		<th>password</th>
		<th>online</th>
	</tr>

<?php
	
	$query = "SELECT * FROM  $usertablename";
	$result = mysql_query($query);
	if(!$result)
	{
		die("Database access failed: " . mysql_error());
	}

	$rows = mysql_num_rows($result);

	for($i = 0; $i < $rows; ++$i)
	{
		echo '<tr>';
		echo '<td>'	. mysql_result($result, $i, 'username')	. '</td>';
		echo '<td>'	. mysql_result($result, $i, 'password')		. '</td>';
        echo mysql_result($result, $i, 'online') == 0 ? '<td>false</td>' : '<td>true</td>';
		echo '</tr>';
	}
?>
</table>




</body>
</html>

<?php
	include 'cleanup.php';
?>