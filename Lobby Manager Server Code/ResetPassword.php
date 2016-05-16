<?php
	$servername = 		"localhost";
	$databasename = 	"Bombertale";
	$usersTablename = 	"Clients";
	$username = 		"someUser";
	$password = 		"wSiphnsu6gco";


	$conn = mysql_connect($servername, $username, $password);

	if (!$conn)
		die('Failed to connect to server');

	if(!mysql_select_db($databasename))
	{
		mysql_close($conn);
		die('Failed to select database');
	}

	if(!(
			isset($_GET['resetId'])
		))
	{
		include 'Cleanup.php';
		die('No reset ID: Try resending reset email');
	}

	$resetId = strip_tags($_GET['resetId']);

	$idLength = strlen($resetId);
	$idLength -= 40;
	$id = substr($resetId, $idLength*-1, $idLength);

	$query = "SELECT * FROM $usersTablename WHERE id = '$id' LIMIT 1";
	$result = mysql_query($query);

	$rows = mysql_num_rows($result);

	if($rows == 0)
	{
		include 'Cleanup.php';
		die('No account found: Try resending reset email');
	}

	$resetIdFromDatabase = sha1(mysql_result($result, 0, 'password'));
	$resetIdFromDatabase .= $id;

	if($resetId != $resetIdFromDatabase)
	{
		include 'Cleanup.php';
		die('Expired reset id');
	}

	include 'Cleanup.php';
?>



<!doctype html>
<html>
<head>
<meta charset="UTF-8">
<title>Reset Password</title>
<link href="_css/main.css" rel="stylesheet" type="text/css">
<script type="text/javascript">
	function resetPassword()
	{
		var newPassword = document.getElementById("new_password_input").value;
		document.getElementById('new_password_input').style.display = 'none';
		document.getElementById('reset_button').style.display = 'none';
		document.getElementById('new_password_label').innerHTML = 'Password updated!';
		document.getElementById('new_password_label').style.color = 'green';
	}
</script>
</head>
<body>
<div id="bombertale_logo"><img src="_assets/BombertaleMenu.png" width="835" height="127" alt=""/></div>

<div id="reset_form">
    <label id="new_password_label">Enter new password</label>
    <br />
    <input id="new_password_input" type="password" />
    <br />
    <button id="reset_button" onclick="resetPassword()">Reset</button>
</div>
</body>
</html>
