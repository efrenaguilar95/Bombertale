<?php

	include 'Setup.php';

	if(!(
			isset($_GET['clientEmail'])
		))
	{
		include 'Cleanup.php';
		die('BD03: Missing arguments');
	}

	$clientEmail = strip_tags($_GET['clientEmail']);

	//check to see if email exsists
	$query = "SELECT * FROM $usersTablename WHERE email = '$clientEmail' LIMIT 1";
	$result = mysql_query($query);
	
	if(!$result)
	{
		include 'Cleanup.php';
		die('BD04: ' . mysql_error());
	}


	$rows = mysql_num_rows($result);

	if($rows == 0)
	{
		include 'Cleanup.php';
		die('BC08: No such email in database');
	}

	$username = mysql_result($result, 0, 'username');
	$id = mysql_result($result, 0, 'id');
	$resetId = sha1(mysql_result($result, 0, 'password'));
	$resetId .= $id;
	$resetLink = "http://apedestrian.com/bombertale/ResetPassword.php?resetId=$resetId";
	$to = $clientEmail;
	$subject = "Lost Bombertale Login";
	$message =
	"Looks like you misplaced your login information! No worries; we got you.
	Username: $username
	Reset password link:
	$resetLink";

	$headers = "From: do-not-reply@apedestrian.com";
	$success = mail($to, $subject, $message, $headers);

	echo $success == true ? 'BC07: Reset email sent' : 'BC09: Failed to send email';

	include 'Cleanup.php';

?>
