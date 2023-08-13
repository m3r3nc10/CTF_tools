$MAX = 63

function dnsbrute {
    param(
        [string]$dominio,
        [string]$wordlist
    )

    if (-not ($dominio) -or -not ($wordlist)) {
        Write-Host "### DnsBrute ###"
        Write-Host "Uso: .\DnsBrute.ps1 -d <dominio> -w <wordlist>"
        return
    }

    $subdominios = Get-Content $wordlist

    foreach ($subdominio in $subdominios) {
        $subdominio = $subdominio.Trim()
        $hostname = "$subdominio.$dominio"
        try {
            if ($subdominio.Length -gt 0 -and $subdominio.Length -le $MAX) {
                $ip = [System.Net.Dns]::GetHostAddresses($hostname)[0].ToString()
                Write-Host "Subdomínio encontrado: $hostname | Endereço IP: $ip"
            }
        }
        catch [System.Net.Sockets.SocketException] {
            # Exceção tratada quando não é possível resolver o nome do host (subdomínio não existe)
        }
        catch {
            # Exceção tratada em caso de erros gerais
        }
    }
}

$dominio = $args[0]
$wordlist = $args[1]

dnsbrute -dominio $dominio -wordlist $wordlist
