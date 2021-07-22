using System;
using System.Collections.Generic;
using System.Threading;
using LOMGAgameClass;
using LOMGAserver;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace LittleTests
{
    class LittleTestsserverProgram
    {

        static void Main(string[] args)
        {
            Thread serverTh = new Thread(serverThread);
            serverTh.Start();

            Console.ReadKey();

            serverTh.Join();
        }

        static void serverThread()
        {
            TcpListener server = new TcpListener(IPAddress.Any, 2003);
            try
            {
                server.Start();

                TcpClient client1 = server.AcceptTcpClient();
                Console.WriteLine("first connected");
                TcpClient client2 = server.AcceptTcpClient();
                Console.WriteLine("second connected");

                NetworkStream stream1 = client1.GetStream();
                NetworkStream stream2 = client2.GetStream();

                stream1.Write(Encoding.Default.GetBytes("true"));
                stream2.Write(Encoding.Default.GetBytes("false"));

                Thread trans1 = new Thread(unused => transmissionThread(stream1, stream2));
                Thread trans2 = new Thread(unused => transmissionThread(stream2, stream1));

                trans1.Start();
                trans2.Start();
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            server.Stop();
        }

        static void transmissionThread(NetworkStream stream1, NetworkStream stream2)
        {
            try
            {
                byte[] data;
                while (true)
                {
                    Console.WriteLine("Waiting to transmit");
                    data = new byte[256];
                    stream1.Read(data);
                    Console.WriteLine("TRANSMITING...");
                    stream2.Write(data);
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }
    }
}
