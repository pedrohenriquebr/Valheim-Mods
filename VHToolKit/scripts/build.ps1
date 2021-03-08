$dev = ($args[0] -eq "-dev")
$BaseDir = "C:\Program Files (x86)\Steam\steamapps\common\Valheim"
if ($dev) {
	$PluginsDir = $BaseDir + "\BepInEx\scripts"
}else {
	$PluginsDir = $BaseDir + "\BepInEx\plugins"
}
$ExecDir = $BaseDir + "\valheim.exe"
dotnet build ..
Copy-Item .\..\bin\Debug\VHToolKit.dll -Destination $PluginsDir