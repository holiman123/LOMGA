
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
                        Console.WriteLine("List request!");
                        // create and send list of games
                        byte[] buffer1;
                        for (int i = 0; i <= gamesList.Count; i++)
                        {
                            if (i != gamesList.Count)
                            {
                                Console.WriteLine("debug");
                                buffer1 = MySerializer.serialize(gamesList[i].gameExemplar);
                            }
                            else
                            {
                                buffer1 = Encoding.Default.GetBytes("end.");
                            }
                            stream.Write(buffer1, 0, buffer1.Length);
                        }
                    }

                    if (Encoding.Default.GetString(data).Split(',')[0] == "choose")
                    {
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
                        break;

                        //======================================
                        //Console.WriteLine("Transmit method started");
                        //data = new byte[1024];
                        //try
                        //{
                        //    //if (((GameClassTTT)game.gameExemplar).turnFlag)
                        //    //{
                        //    //    Console.WriteLine("waiting to change!");
                        //    //    stream2.Read(data);
                        //    //    Console.WriteLine("Game changing!");
                        //    //    stream1.Write(data);
                        //    //}
                        //    while (true)
                        //    {
                        //        data = new byte[1024];
                        //        Console.WriteLine("hostStream canRead:" + gamesList[choosedGameIndex].hostStream.CanRead);
                        //        Console.WriteLine("waiting to change!");
                        //        gamesList[choosedGameIndex].hostStream.Read(data, 0, 471);
                        //        Console.WriteLine("clientStream canWrite:" + gamesList[choosedGameIndex].clientStream.CanWrite);
                        //        Console.WriteLine("Game changing!");
                        //        gamesList[choosedGameIndex].clientStream.Write(data, 0, 471);
                        //        Console.WriteLine("clientStream canRead:" + gamesList[choosedGameIndex].clientStream.CanRead);
                        //        Console.WriteLine("waiting to change!");
                        //        gamesList[choosedGameIndex].clientStream.Read(data, 0, 471);
                        //        Console.WriteLine("hostStream canWrite:" + gamesList[choosedGameIndex].hostStream.CanWrite);
                        //        Console.WriteLine("Game changing!");
                        //        gamesList[choosedGameIndex].hostStream.Write(data, 0, 471);
                        //    }
                        //}
                        //catch (Exception e) { Console.WriteLine(e.Message); }
                        //gamesList[choosedGameIndex].clientStream.Close();
                        //gamesList[choosedGameIndex].hostStream.Close();
                        //Console.WriteLine("both streams have been closed.");
                        //======================================
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
                while (true)
                {
                    Console.WriteLine("waiting to change!");
                    data = new byte[472]; 
                    stream1.Read(data);
                    Console.WriteLine("Game changing!");
                    stream2.Write(data);
                    //while (true)
                    //{
                    //    data = new byte[1024];
                    //    Console.WriteLine("waiting to change! of " + stream1.CanRead);
                    //    stream1.Read(data);
                    //    Console.WriteLine("Game changing!");
                    //    stream2.Write(data);
                    //    Console.WriteLine("waiting to change!");
                    //    stream2.Read(data);
                    //    Console.WriteLine("Game changing!");
                    //    stream1.Write(data);
                    //}
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            //game.clientStream.Close();
            //game.hostStream.Close();
            Console.WriteLine("both streams have been closed.");
        }
    }
}
