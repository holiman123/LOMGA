using System;
using LOMGAgameClass;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace LOMGAserver
{
    class Program
    {
        public static List<OnlineGame> gamesList = new List<OnlineGame>();

        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(System.Net.IPAddress.Any, 2003);
            server.Start();
            Console.WriteLine("Server started...");

            TcpClient clientBuffer;
            NetworkStream streamBuffer;
            Thread threadBuffer;
            while (true)
            {
                clientBuffer = server.AcceptTcpClient();
                Console.WriteLine("client connected");
                streamBuffer = clientBuffer.GetStream();

                threadBuffer = new Thread(new ParameterizedThreadStart(connection));
                threadBuffer.Start(streamBuffer);
            }
        }

        public static void connection(object stream)
        {
            Console.WriteLine("connection thread started:");
            NetworkStream firstStream = (NetworkStream)stream;

            byte[] data = new byte[128];
            Console.WriteLine("debug");
            firstStream.Read(data, 0, data.Length);

            if (Encoding.Default.GetString(data).Split(',')[0] == "start")
            {
                Console.WriteLine("new game starting");
                //TODO: choose game by second part of string message from host-client
                GameClassTTT game = new GameClassTTT();
                game.hostStream = firstStream;
                game.gameIndex = gamesList.Count;
                gamesList.Add(game);
                Console.WriteLine("new game added to list           " + JsonSerializer.Serialize(gamesList[0]));
            }

            Console.WriteLine(Encoding.Default.GetString(data));

            if (Encoding.Default.GetString(data).Split(',')[0] == "list")
            {
                Console.WriteLine("list request\t" + JsonSerializer.Serialize(gamesList));

                // send list of games
                byte[] dataBuffer; 
                dataBuffer = Encoding.Default.GetBytes(JsonSerializer.Serialize(gamesList));
                Console.WriteLine(dataBuffer.Length);
                Console.WriteLine("\n\n\n\n" + JsonSerializer.Serialize(gamesList));
                firstStream.Write(dataBuffer, 0, data.Length);

                // read choosen game
                dataBuffer = new byte[512];
                firstStream.Read(dataBuffer, 0, dataBuffer.Length);
                OnlineGame choosedGame = JsonSerializer.Deserialize<OnlineGame>(Encoding.Default.GetString(dataBuffer));
                int choosenGameIndex = 0;
                foreach(OnlineGame g in gamesList)
                {
                    choosenGameIndex++;
                    if(g == choosedGame)
                        break;
                }
                // change choosen game and send it to both of players. (with ready to play flag)
                gamesList[choosenGameIndex].clientStream = firstStream;
                gamesList[choosenGameIndex].isGameready = true;
                dataBuffer = Encoding.Default.GetBytes(JsonSerializer.Serialize(gamesList[choosenGameIndex]));
                gamesList[choosenGameIndex].hostStream.Write(data, 0, dataBuffer.Length);
                gamesList[choosenGameIndex].clientStream.Write(data, 0, dataBuffer.Length);
                // change game in list to started.
                gamesList[choosenGameIndex].isStarted = true;
                //TODO: start endless cycle to send/read new changed game
                Thread hostTransmitThread = new Thread(unused => transmitMethod(gamesList[choosenGameIndex].hostStream, gamesList[choosenGameIndex].clientStream));
                Thread clientTransmitThread = new Thread(unused => transmitMethod(gamesList[choosenGameIndex].clientStream, gamesList[choosenGameIndex].hostStream));
                hostTransmitThread.Start();
                clientTransmitThread.Start();
            }
        }

        public static void transmitMethod(NetworkStream streamHost, NetworkStream streamClient)
        {
            byte[] data = new byte[512];
            while (true)
            {
                data = new byte[512];
                streamHost.Read(data, 0, data.Length);
                streamClient.Write(data, 0, data.Length);
            }
        }

    }
}
