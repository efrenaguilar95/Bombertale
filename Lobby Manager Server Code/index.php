<?php
include 'db_info.php';

// Create connection
$conn = mysql_connect($servername, $username, $password);

// Check connection
if (!$conn)
{
    die("Connection failed: " . mysql_error());
} 

mysql_select_db($databasename)
	or die("Unable to select database" . mysql_error());

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
		<th>game_name</th>
		<th>host_ip</th>
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
		echo '<td>'	. mysql_result($result, $i, 'game_name')	. '</td>';
		echo '<td>'	. mysql_result($result, $i, 'host_ip')		. '</td>';
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




</body>
</html>