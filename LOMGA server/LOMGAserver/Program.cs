
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
            // Server start

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

                threadBuffer = new Thread(unused => connection(streamBuffer));
                threadBuffer.Start();
            }
        }

        public static void connection(NetworkStream stream)
        {
            try
            {
                Console.WriteLine("connection thread started:");
                NetworkStream firstStream = (NetworkStream)stream;

                while (true)
                {
                    byte[] data = new byte[512];
                    stream.Read(data, 0, data.Length);

                    if (Encoding.Default.GetString(data).Split(',')[0] == "start")
                    {
                        Console.WriteLine("new game starting");

                        // choose game by second part of string message from host-client
                        OnlineGame onlineGame = null;
                        int recivedGameType = Convert.ToInt32(Encoding.Default.GetString(data).Split(',')[1]);
                        switch (recivedGameType)
                        {
                            case 0:   // TTT game
                                GameClassTTT tempTTTGameClass = new GameClassTTT();
                                onlineGame = new OnlineGame();
                                onlineGame.hostStream = stream;
                                onlineGame.gameExemplar = tempTTTGameClass;
                                break;
                        }

                        // add new game to games list
                        if (onlineGame != null)
                        {
                            onlineGame.gameExemplar.gameIndex = gamesList.Count;
                            gamesList.Add(onlineGame);
                            Console.WriteLine("new game added to list");
                            break;
                        }
                    }

                    if (Encoding.Default.GetString(data).Split(',')[0] == "list")
                    {
                        // create and send list of games:

                        Console.WriteLine("List request!");
                        byte[] listBuffer;
                        for (int i = 0; i <= gamesList.Count; i++)
                        {
                            if (i != gamesList.Count)
                            {
                                Console.WriteLine("debug");
                                listBuffer = MySerializer.serialize(gamesList[i].gameExemplar);
                            }
                            else
                            {
                                listBuffer = Encoding.Default.GetBytes("end.");
                            }
                            stream.Write(listBuffer, 0, listBuffer.Length);
                        }
                    }

                    if (Encoding.Default.GetString(data).Split(',')[0] == "choose")
                    {
                        // choosing game from list:

                        // read choosen game
                        stream.Read(data, 0, data.Length);
                        Game readedGame = (Game)MySerializer.deserialize(data);
                        // compare deserialize game to list, and find choosed game
                        int choosedGameIndex = -1;
                        foreach (OnlineGame g in gamesList)
                        {
                            if (g.gameExemplar == readedGame)
                                choosedGameIndex = g.gameExemplar.gameIndex;
                        }
                        Console.WriteLine("choosed game with {0} index", choosedGameIndex);
                        Console.WriteLine("list of games count:" + gamesList.Count);

                        // change choosen game parametrs (set client stream) and send it to both of players. (with ready to play flag)
                        gamesList[choosedGameIndex].clientStream = stream;
                        gamesList[choosedGameIndex].gameExemplar.isGameready = true;
                        gamesList[choosedGameIndex].gameExemplar.isStarted = true;


                        // send to both
                        byte[] buffer = MySerializer.serialize(gamesList[choosedGameIndex].gameExemplar);
                        gamesList[choosedGameIndex].clientStream.Write(buffer, 0, buffer.Length);
                        //======================================
                        ((GameClassTTT)gamesList[choosedGameIndex].gameExemplar).areYouTurningFirst = true; // TODO: make random chooser of first turn
                        buffer = MySerializer.serialize(gamesList[choosedGameIndex].gameExemplar);
                        //======================================
                        gamesList[choosedGameIndex].hostStream.Write(buffer, 0, buffer.Length);

                        //start endless cycle to send/read new changed game
                        Thread hostTransmitThread = new Thread(unused => transmitMethod(gamesList[choosedGameIndex].hostStream, gamesList[choosedGameIndex].clientStream));
                        hostTransmitThread.Start();
                        Thread clientTransmitThread = new Thread(unused => transmitMethod(gamesList[choosedGameIndex].clientStream, gamesList[choosedGameIndex].hostStream));
                        clientTransmitThread.Start();

                        // Waiting threads to end ecouse of error or game end
                        hostTransmitThread.Join();
                        clientTransmitThread.Join();

                        // clear streams
                        gamesList[choosedGameIndex].hostStream.Close();
                        gamesList[choosedGameIndex].clientStream.Close();

                        // removing game from game list
                        gamesList.RemoveAt(choosedGameIndex);

                        Console.WriteLine("Game end! streams cleared, game removed from list.");
                        break;
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

        static void transmitMethod(NetworkStream stream1, NetworkStream stream2)
        {
            Console.WriteLine("Transmit method started");
            try
            {
                byte[] data = new byte[1024];
                while (stream2.CanWrite)
                {
                    Console.WriteLine("waiting to change!");
                    data = new byte[472]; 
                    stream1.Read(data);
                    Console.WriteLine("Game changing!");
                    stream2.Write(data);
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }
    }
}
