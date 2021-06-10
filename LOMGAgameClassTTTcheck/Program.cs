using System;
using LOMGAgameClass;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using LOMGAserver;
using System.Collections.Generic;

namespace LOMGAgameClassTTTcheck
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Game> gamesList = new List<Game>();

            TcpClient clientHost = new TcpClient();
            clientHost.Connect(System.Net.IPAddress.Parse("127.0.0.1"), 2003);
            NetworkStream streamHost = clientHost.GetStream();

            byte[] data = Encoding.Default.GetBytes("start,0");
            streamHost.Write(data, 0, data.Length);

            TcpClient clientClient = new TcpClient();
            clientClient.Connect(System.Net.IPAddress.Parse("127.0.0.1"), 2003);
            NetworkStream streamClient = clientClient.GetStream();

            data = Encoding.Default.GetBytes("list,");
            Thread.Sleep(2000);
            streamClient.Write(data, 0, data.Length);
            byte[] data1;
            while(true)
            {
                data1 = new byte[512];
                streamClient.Read(data1, 0, data1.Length);

                if (Encoding.Default.GetString(data1).Remove(4) == "end.")
                    break;

                gamesList.Add((Game)MySerializer.deserialize(data1));
                Console.WriteLine("recived another one game!");
            }
            Console.WriteLine("recived games count: " + gamesList.Count);

            data = MySerializer.serialize(gamesList[0]);
            streamClient.Write(data, 0 , data.Length);

            data = new byte[1024];
            data1 = new byte[1024];

            streamClient.Read(data);
            streamHost.Read(data1);

            Game myGame1 = (Game)MySerializer.deserialize(data);
            Game myGame2 = (Game)MySerializer.deserialize(data1);

            ((GameClassTTT)myGame1).turn(1,1);

            streamClient.Write(MySerializer.serialize(myGame1));

            data1 = new byte[1024];
            streamHost.Read(data1);
            myGame2 = (Game)MySerializer.deserialize(data1);

            Console.WriteLine(((GameClassTTT)myGame2).field[1, 1]);

            Console.ReadKey();
        }
    }
}
