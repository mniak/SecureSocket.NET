using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecureSockets;
using System.Net;
using System.Threading;
using System.Net.Sockets;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            RunClientInSynchronousMode();
        }


        private static void RunClientInSynchronousMode()
        {
            int timer = 1000;
            byte[] buffer = new byte[1024];
            int receivedBytes;

            SecureSocket socket = new SecureSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Connecting to server in {0} seconds", timer / 1000D);
            Thread.Sleep(timer);
            socket.Connect("127.0.0.1", 9999);
            Console.WriteLine("Connected to server");
            while (socket.Connected)
            {
                // Leitura
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("> ");
                string line = Console.ReadLine();
                socket.Send(Encoding.UTF8.GetBytes(line));

                // Escrita
                Console.ForegroundColor = ConsoleColor.Yellow;
                do
                {
                    receivedBytes = socket.Receive(buffer);
                    byte[] temp = new byte[receivedBytes];
                    Array.Copy(buffer, 0, temp, 0, receivedBytes);
                    Console.WriteLine("< {0}", Encoding.UTF8.GetString(temp));
                }
                while (receivedBytes < buffer.Length && socket.Available > 0);
                Thread.Sleep(50);

            }
            Console.WriteLine("Disconnected. Finishing...");
        }
    }
}
