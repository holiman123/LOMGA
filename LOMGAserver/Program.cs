
// MAIN TODO:       -rewrite ALL serializers to new statement (that one with memoryStream).


using System;
using LOMGAgameClass;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Text;

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

            byte[] data = new byte[512];
            firstStream.Read(data, 0, data.Length);

            if (Encoding.Default.GetString(data).Split(',')[0] == "start")
            {
                Console.WriteLine("new game starting");

                // choose game by second part of string message from host-client
                OnlineGame onlineGame = null;
                Convert.ToInt32(Encoding.Default.GetString(data).Split(',')[1]);
                switch (Convert.ToInt32(Encoding.Default.GetString(data).Split(',')[1]))
                {
                    case 0:   // TTT game
                        GameClassTTT tempTTTGameClass = new GameClassTTT();
                        tempTTTGameClass.gameType = -1;
                        onlineGame = new OnlineGame();
                        onlineGame.hostStream = firstStream;
                        onlineGame.gameExemplar = tempTTTGameClass;
                        break;
                }

                // add new game to games list
                if (onlineGame != null)
                {
                    onlineGame.gameExemplar.gameIndex = gamesList.Count;
                    gamesList.Add(onlineGame);
                    Console.WriteLine("new game added to list");
                }
            }

            if (Encoding.Default.GetString(data).Split(',')[0] == "list")
            {
                Console.WriteLine("List request!");
                // create and send list of games
                byte[] buffer;
                for (int i = 0; i <= gamesList.Count; i++)
                {
                    if (i != gamesList.Count)
                    {
                        Console.WriteLine("debug");
                        buffer = MySerializer.serialize(gamesList[i].gameExemplar);
                    }
                    else
                    {
                        buffer = Encoding.Default.GetBytes("end.");
                    }
                    firstStream.Write(buffer, 0, buffer.Length);
                }

                // read choosen game
                firstStream.Read(data, 0, data.Length);
                Game readedGame = (Game)MySerializer.deserialize(data);
                // compare deserialize game to list, and find choosed game
                int choosedGameIndex = -1;
                foreach (OnlineGame g in gamesList)
                {
                    if (g.gameExemplar == readedGame)
                        choosedGameIndex = g.gameExemplar.gameIndex;
                }

                // change choosen game parametrs (set client stream) and send it to both of players. (with ready to play flag)
                gamesList[choosedGameIndex].clientStream = firstStream;
                gamesList[choosedGameIndex].gameExemplar.isGameready = true;
                gamesList[choosedGameIndex].gameExemplar.isStarted = true;
               
                // send to both
                buffer = MySerializer.serialize(gamesList[choosedGameIndex].gameExemplar);
                gamesList[choosedGameIndex].clientStream.Write(buffer, 0, buffer.Length);
                gamesList[choosedGameIndex].hostStream.Write(buffer, 0, buffer.Length);

                //TODO: start endless cycle to send/read new changed game
                Thread hostTransmitThread = new Thread(unused => transmitMethod(gamesList[choosedGameIndex].clientStream, gamesList[choosedGameIndex].hostStream, gamesList[choosedGameIndex]));
                Thread clientTransmitThread = new Thread(unused => transmitMethod(gamesList[choosedGameIndex].clientStream, gamesList[choosedGameIndex].hostStream, gamesList[choosedGameIndex]));
                hostTransmitThread.Start();
                clientTransmitThread.Start();
            }
        }

        public static void transmitMethod(NetworkStream streamHost, NetworkStream streamClient, OnlineGame game)
        {
            Console.WriteLine("Transmit method started");
            byte[] data;
            while (true)
            {
                data = new byte[1024];
                try
                {
                    streamHost.Read(data, 0, data.Length);
                    streamClient.Write(data, 0, data.Length);
                }
                catch (Exception e) { break; }
            }
            game.clientStream.Close();
            game.hostStream.Close();
            Console.WriteLine("both streams have been closed.");
        }

    }
}
