using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecureSockets;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] buffer = new byte[1024];
            int bytesRecebidos;

            SecureSocket socket = new SecureSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999));
            socket.Listen(10);
            Console.WriteLine("Socket ligado");
            Console.WriteLine("Aguardando conexoes...");
            while (true)
            {
                var scli = socket.Accept();
                Console.WriteLine("Conexao recebida.");
                while (scli.Connected)
                {
                    Console.WriteLine("Lendo dados...");
                    do
                    {
                        bytesRecebidos = scli.Receive(buffer);
                        byte[] temp = new byte[bytesRecebidos];
                        Array.Copy(buffer, 0, temp, 0, bytesRecebidos);
                        Console.WriteLine("Dados lidos: \"{0}\"", Encoding.UTF8.GetString(temp));
                        scli.Send(Encoding.UTF8.GetBytes(string.Format("ACK: \"{0}\"", Encoding.UTF8.GetString(temp))));
                    }
                    while (bytesRecebidos < buffer.Length && scli.Available > 0);

                    Thread.Sleep(50);
                }
                Console.WriteLine("Desconectado. Aguardando nova conexão...");
            }
        }
    }
}