<!doctype html>
<html>
<head>
<meta charset="UTF-8">
<title>Reset Password</title>
<link href="_css/main.css" rel="stylesheet" type="text/css">
<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.2/jquery.min.js"></script>
<script type="text/javascript">
	function getServers()
	{
		var getServersLink = 'http://apedestrian.com/bombertale/GetServerScores.php';
		$("#hosted_servers").load(getServersLink);
	}
</script>
</head>
<body>
<div id="bombertale_logo"><img src="_assets/BombertaleMenu.png" width="835" height="127" alt=""/></div>

<div id="hosted_servers">
	<label id="hosted_servers_label">Select a game to watch</label>
	<br />
</div>
</body>
</html>
