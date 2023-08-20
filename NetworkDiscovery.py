import re
import sys
import subprocess


#net = sys.argv[1]
net = "192.168.10"
ips = [i for i in range(255)]
respondeu = "bytes="

for ip in ips:
    comando = f"ping -n 1 {net}.{ip}"
    try:
        ping = subprocess.run(comando, shell=True, text=True, capture_output=True)
        saida = ping.stdout
        if respondeu in saida:
            ip_ativo = re.search(r'\d+\.\d+\.\d+\.\d+', saida)
            if ip_ativo:
                ip_ativo = ip_ativo.group()
                print(ip_ativo)
    except KeyboardInterrupt:
        sys.exit(0)
