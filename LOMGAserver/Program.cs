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
            firstStream.Read(data, 0, data.Length);

            if (Convert.ToString(data).Split(',')[0] == "start")
            {
                Console.WriteLine("new game starting");
                GameClassTTT game = new GameClassTTT();
                game.hostStream = firstStream;
                game.gameIndex = gamesList.Count;
                gamesList.Add(game);
            }

            if (Convert.ToString(data) == "list")
            {
                Console.WriteLine("list request");

                // send list of games
                byte[] dataBuffer; 
                dataBuffer = Encoding.Default.GetBytes(JsonSerializer.Serialize(gamesList));
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
                //TODO: change choosen game and send it to both of players. (with ready to play flag)
                gamesList[choosenGameIndex].clientStream = firstStream;
                gamesList[choosenGameIndex].isGameready = true;
                //TODO: change game in list to started.
                //TODO: start endless cycle to send/read new changed game
            }
        }
    }
}
