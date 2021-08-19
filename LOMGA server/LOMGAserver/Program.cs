
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
            Thread threadBuffer;
            while (true)
            {
                clientBuffer = server.AcceptTcpClient();
                Console.WriteLine("client connected");

                threadBuffer = new Thread(unused => connection(clientBuffer));
                threadBuffer.Start();
            }
        }

        public static void connection(TcpClient client)
        {
            try
            {
                Console.WriteLine("connection thread started:");
                NetworkStream stream = client.GetStream();

                while (true)
                {
                    byte[] data = new byte[1024];
                    stream.Read(data, 0, data.Length);

                    if (Encoding.Default.GetString(data).Split(',')[0] == "start")
                    {
                        Console.WriteLine("new game starting");

                        // choose game by second part of string message from host-client
                        OnlineGame onlineGame = null;
                        //int recivedGameType = Convert.ToInt32(Encoding.Default.GetString(data).Split(',')[1]);

                        stream.Read(data, 0, data.Length);
                        Game newGame = (Game)MySerializer.deserialize(data);

                        switch (newGame.gameType)
                        {
                            case 1:   // TTT game
                                onlineGame = new OnlineGame();
                                onlineGame.hostClient = client;
                                onlineGame.hostStream = stream;
                                onlineGame.gameExemplar = newGame;
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
                        gamesList[choosedGameIndex].clientClient = client;
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
                        Thread hostTransmitThread = new Thread(unused => transmitMethod(gamesList[choosedGameIndex].hostStream, gamesList[choosedGameIndex].clientStream, gamesList[choosedGameIndex].hostClient));
                        hostTransmitThread.Start();
                        Thread clientTransmitThread = new Thread(unused => transmitMethod(gamesList[choosedGameIndex].clientStream, gamesList[choosedGameIndex].hostStream, gamesList[choosedGameIndex].clientClient));
                        clientTransmitThread.Start();

                        // Waiting threads to end becouse of error or game end
                        hostTransmitThread.Join();
                        clientTransmitThread.Join();

                        // clear streams
                        gamesList[choosedGameIndex].hostStream.Close();
                        gamesList[choosedGameIndex].clientStream.Close();
                        gamesList[choosedGameIndex].hostClient.Close();
                        gamesList[choosedGameIndex].clientClient.Close();

                        // removing game from game list
                        gamesList.RemoveAt(choosedGameIndex);

                        Console.WriteLine("Game end! Streams cleared, game removed from list.");
                        break;
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

        static void transmitMethod(NetworkStream stream1, NetworkStream stream2, TcpClient client)
        {
            Console.WriteLine("Transmit method started");
            try
            {
                byte[] data = new byte[1024];
                while (client.Connected)
                {
                    Console.WriteLine("waiting to change!");
                    data = new byte[1024]; 
                    stream1.Read(data);

                    if ((Game)MySerializer.deserialize(data) is null)
                        throw new Exception();

                    Console.WriteLine("Game changing!");
                    stream2.Write(data);
                }
            }
            catch (Exception e) 
            { 
                Console.WriteLine(e.Message);
                stream1.Close();
                stream2.Close();
                client.Close();
            }
        }
    }
}
