using System;
using LOMGAgameClass;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LOMGAgameClassTTTcheck
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient clientHost = new TcpClient();
            clientHost.Connect(System.Net.IPAddress.Parse("127.0.0.1"), 2003);
            NetworkStream streamHost = clientHost.GetStream();

            byte[] data = Encoding.Default.GetBytes("start,0");
            streamHost.Write(data, 0, data.Length);

            TcpClient clientClient = new TcpClient();
            clientClient.Connect(System.Net.IPAddress.Parse("127.0.0.1"), 2003);
            NetworkStream streamClient = clientClient.GetStream();

            data = Encoding.Default.GetBytes("list,");
            Thread.Sleep(5000);
            streamClient.Write(data, 0, data.Length);

            Console.ReadKey();
        }
    }
}
