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
        static SecureSocket socket;
        static void Main(string[] args)
        {
            //RunClientInSynchronousMode();
            RunClientInAsynchronousMode();
        }

        private static void RunClientInAsynchronousMode()
        {
            int timer = 1000;
            byte[] buffer = new byte[1024];
            int bytesRecebidos;

            socket = new SecureSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Conectando ao servidor em {0} segundos", timer / 1000D);
            Thread.Sleep(timer);
            Console.WriteLine("A partir daqui ele tem que conectar");
            socket.BeginConnect("127.0.0.1", 8080, ConnectCallback, null);
            //socket.Connect("127.0.0.1", 9998);
            //Console.WriteLine("Conectado ao servidor");
            //while (socket.Connected)
            //{
            //    // Leitura
            //    Console.ForegroundColor = ConsoleColor.White;
            //    Console.Write("> ");
            //    string linha = Console.ReadLine();
            //    socket.Send(Encoding.UTF8.GetBytes(linha));

            //    // Escrita
            //    Console.ForegroundColor = ConsoleColor.Yellow;
            //    do
            //    {
            //        bytesRecebidos = socket.Receive(buffer);
            //        byte[] temp = new byte[bytesRecebidos];
            //        Array.Copy(buffer, 0, temp, 0, bytesRecebidos);
            //        Console.WriteLine("< {0}", Encoding.UTF8.GetString(temp));
            //    }
            //    while (bytesRecebidos < buffer.Length && socket.Available > 0);
            //    Thread.Sleep(50);

            //}
            //Console.WriteLine("Desconectado. Finalizando...");
            while (true)
            {
                Thread.Sleep(10 * 1000);
            }
        }
        static void ConnectCallback(IAsyncResult ar)
        {
            Console.WriteLine("Parece que conectou...");
            socket.EndConnect(ar);
        }
        //private static void RunClientInSynchronousMode()
        //{
        //    int timer = 1000;
        //    byte[] buffer = new byte[1024];
        //    int bytesRecebidos;

        //    SecureSocket socket = new SecureSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //    Console.WriteLine("Conectando ao servidor em {0} segundos", timer / 1000D);
        //    Thread.Sleep(timer);
        //    socket.Connect("127.0.0.1", 9999);
        //    Console.WriteLine("Conectado ao servidor");
        //    while (socket.Connected)
        //    {
        //        // Leitura
        //        Console.ForegroundColor = ConsoleColor.White;
        //        Console.Write("> ");
        //        string linha = Console.ReadLine();
        //        socket.Send(Encoding.UTF8.GetBytes(linha));

        //        // Escrita
        //        Console.ForegroundColor = ConsoleColor.Yellow;
        //        do
        //        {
        //            bytesRecebidos = socket.Receive(buffer);
        //            byte[] temp = new byte[bytesRecebidos];
        //            Array.Copy(buffer, 0, temp, 0, bytesRecebidos);
        //            Console.WriteLine("< {0}", Encoding.UTF8.GetString(temp));
        //        }
        //        while (bytesRecebidos < buffer.Length && socket.Available > 0);
        //        Thread.Sleep(50);

        //    }
        //    Console.WriteLine("Desconectado. Finalizando...");
        //}
    }
}
