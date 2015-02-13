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
            int receivedBytes;

            SecureSocket socket = new SecureSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999));
            socket.Listen(10);
            Console.WriteLine("Socket started");
            Console.WriteLine("Waiting connections...");
            while (true)
            {
                var scli = socket.Accept();
                Console.WriteLine("Connection received.");
                while (scli.Connected)
                {
                    Console.WriteLine("Reading data...");
                    do
                    {
                        receivedBytes = scli.Receive(buffer);
                        byte[] temp = new byte[receivedBytes];
                        Array.Copy(buffer, 0, temp, 0, receivedBytes);
                        Console.WriteLine("Data read: \"{0}\"", Encoding.UTF8.GetString(temp));
                        scli.Send(Encoding.UTF8.GetBytes(string.Format("ACK: \"{0}\"", Encoding.UTF8.GetString(temp))));
                    }
                    while (receivedBytes < buffer.Length && scli.Available > 0);

                    Thread.Sleep(50);
                }
                Console.WriteLine("Disconnected. Waiting new connection...");
            }
        }
    }
}