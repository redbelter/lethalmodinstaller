#powershell blows

$steamPath = Get-ItemProperty -Path Registry::HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Valve\Steam -Name InstallPath
$steamPath = $steamPath.InstallPath
if(-not (Test-Path $steamPath -PathType Leaf)){

}

$myJson = Get-Content .\currentlist.txt -Raw | ConvertFrom-Json 
$myJson