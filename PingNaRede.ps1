# PingNaRede
param($p1)
if(!$p1){
    Write-Host "Script PingNaRede"
    Write-Host "Uso: .\PingNaRede.ps1 192.168.X"
} else {
    foreach($ip in 1..254) {
    try{
        $resp = ping -n 1 "$p1.$ip" | Select-String "bytes=32"
        $resp.Line.split(" ")[2] -replace ":",""
    } catch {}
    }
}