using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace LittleTestsClient
{
    class LittleTestsclientProgram
    {
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse("127.0.0.1"), 2003);

            NetworkStream stream = client.GetStream();

            bool jopa = false;
            byte[] data = new byte[16];
            stream.Read(data);
            if (Encoding.Default.GetString(data).StartsWith("true"))
                jopa = true;

            reciveThread(stream, jopa);
            //Thread thread = new Thread(unused => reciveThread(stream, jopa));
            //thread.Start();

            Console.ReadKey();
        }

        static void reciveThread(NetworkStream stream, bool jopa)
        {
            try
            {
                if (jopa)
                {
                    Console.WriteLine("writePleas:");
                    string message = Console.ReadLine();
                    stream.Write(Encoding.Default.GetBytes(message + ","));
                }

                byte[] data;
                while (true)
                {
                    data = new byte[256];
                    stream.Read(data);
                    Console.WriteLine(Encoding.Default.GetString(data).Split(',')[0] + "\nwritePleas:");
                    string message = Console.ReadLine();
                    stream.Write(Encoding.Default.GetBytes(message + ","));
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }
    }
}
