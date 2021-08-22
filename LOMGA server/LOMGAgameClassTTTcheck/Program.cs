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
        public static void hardTestAdditionalthread(NetworkStream stream, Game game)
        {
            byte[] turnData;
            if (((GameClassTTT)game).areYouTurningFirst)
            {
                ((GameClassTTT)game).areYouTurningFirst = false;
                // show field
                Console.WriteLine("{0} {1} {2}", ((GameClassTTT)game).field[0, 0], ((GameClassTTT)game).field[0, 1], ((GameClassTTT)game).field[0, 2]);
                Console.WriteLine("{0} {1} {2}", ((GameClassTTT)game).field[1, 0], ((GameClassTTT)game).field[1, 1], ((GameClassTTT)game).field[1, 2]);
                Console.WriteLine("{0} {1} {2}", ((GameClassTTT)game).field[2, 0], ((GameClassTTT)game).field[2, 1], ((GameClassTTT)game).field[2, 2]);

                // turning
                Console.WriteLine("make your turn");
                string turnString = Console.ReadLine();

                // send new turn
                ((GameClassTTT)game).turn(Convert.ToInt32(turnString.Split(' ')[0]), Convert.ToInt32(turnString.Split(' ')[1]));
                turnData = MySerializer.serialize(game);
                stream.Write(turnData, 0, turnData.Length);
                Console.WriteLine("new turn sended " + turnData.Length);
            }

            byte[] data;
            while(true)
            {
                try
                {
                    // get opponent turn
                    data = new byte[1024];
                    Console.WriteLine("Wait untill opponent make turn...");
                    stream.Read(data, 0, 1024);
                    game = (Game)MySerializer.deserialize(data);

                    // show field
                    Console.WriteLine("{0} {1} {2}", ((GameClassTTT)game).field[0, 0], ((GameClassTTT)game).field[0, 1], ((GameClassTTT)game).field[0, 2]);
                    Console.WriteLine("{0} {1} {2}", ((GameClassTTT)game).field[1, 0], ((GameClassTTT)game).field[1, 1], ((GameClassTTT)game).field[1, 2]);
                    Console.WriteLine("{0} {1} {2}", ((GameClassTTT)game).field[2, 0], ((GameClassTTT)game).field[2, 1], ((GameClassTTT)game).field[2, 2]);

                    // turning
                    Console.WriteLine("make your turn");
                    string turnString = Console.ReadLine();

                    // send new turn
                    ((GameClassTTT)game).turn(Convert.ToInt32(turnString.Split(' ')[0]), Convert.ToInt32(turnString.Split(' ')[1]));
                    turnData = MySerializer.serialize(game);
                    stream.Write(turnData, 0, turnData.Length);
                    Console.WriteLine("new turn sended " + MySerializer.serialize(game).Length);
                }
                catch (Exception e) { Console.WriteLine(e.Message); Console.ReadKey(); }
            }
        }

        static void Main(string[] args)
        {
            hardTest();

            //Console.ReadKey();
        }

        static void hardTest()
        {
            // Create tcp client and connect
            Console.WriteLine("Connecting...");
            TcpClient client = new TcpClient();
            client.Connect(System.Net.IPAddress.Parse("127.0.0.1"), 2003);
            NetworkStream stream = client.GetStream();
            List<Game> gamesList = new List<Game>();

            while (true)
            {
                // get command:
                Console.WriteLine("Write command:");
                string message = Console.ReadLine();

                if (message == "start")
                {
                    Console.WriteLine("Write type of game ( 0 - TicTacToy )");
                    message += "," + Console.ReadLine();
                    byte[] startData = Encoding.Default.GetBytes(message);
                    stream.Write(startData, 0, startData.Length);

                    GameClassTTT tempTTT = new GameClassTTT();
                    tempTTT.accounts.Add(new Account("check nickname", tempTTT.accounts.Count));
                    startData = MySerializer.serialize(tempTTT);
                    stream.Write(startData, 0, startData.Length);

                    break;
                }

                if (message == "list")
                {
                    message += ",";
                    byte[] listData = Encoding.Default.GetBytes(message);
                    stream.Write(listData, 0, listData.Length);

                    while (true)
                    {
                        listData = new byte[1024];
                        stream.Read(listData, 0, 1024);

                        if (Encoding.Default.GetString(listData).Remove(4) == "end.")
                            break;

                        gamesList.Add((Game)MySerializer.deserialize(listData));
                        Console.WriteLine("recived " + gamesList[gamesList.Count - 1].gameType + "-type game!");
                        stream.Write(new byte[] { 0 }, 0, 1);
                    }
                    Console.WriteLine("recived games count: " + gamesList.Count);
                }

                if (message == "choose")
                {
                    byte[] chooseData = Encoding.Default.GetBytes("choose,");
                    stream.Write(chooseData, 0, chooseData.Length);

                    Console.WriteLine("Write number of game you want to connect:");
                    int choosedGameIndex = Convert.ToInt32(Console.ReadLine());

                    gamesList[choosedGameIndex].accounts.Add(new Account("nick 1", gamesList[choosedGameIndex].accounts.Count));
                    chooseData = MySerializer.serialize(gamesList[choosedGameIndex]);
                    stream.Write(chooseData, 0, chooseData.Length);

                    break;
                }
            }

            byte[] data = new byte[1024];
            stream.Read(data, 0, 1024);
            Game myGame = (Game)MySerializer.deserialize(data);

            Console.WriteLine("Game started!");

            hardTestAdditionalthread(stream, myGame);
        }

        static void simpleTest()
        {
            List<Game> gamesList = new List<Game>();

            TcpClient clientHost = new TcpClient();
            clientHost.Connect(System.Net.IPAddress.Parse("127.0.0.1"), 2003);
            NetworkStream streamHost = clientHost.GetStream();

            // send message to start new game, ( 0 - means TTT )
            byte[] data = Encoding.Default.GetBytes("start,0");
            streamHost.Write(data, 0, data.Length);

            // another player connected
            TcpClient clientClient = new TcpClient();
            clientClient.Connect(System.Net.IPAddress.Parse("127.0.0.1"), 2003);
            NetworkStream streamClient = clientClient.GetStream();

            // second player getting list
            data = Encoding.Default.GetBytes("list,");
            Thread.Sleep(2000);
            streamClient.Write(data, 0, data.Length);
            byte[] data1;
            while (true)
            {
                data1 = new byte[512];
                streamClient.Read(data1, 0, data1.Length);

                if (Encoding.Default.GetString(data1).Remove(4) == "end.")
                    break;

                gamesList.Add((Game)MySerializer.deserialize(data1));
                Console.WriteLine("recived another one game!");
            }
            Console.WriteLine("recived games count: " + gamesList.Count);

            // choose first game from list
            data = Encoding.Default.GetBytes("choose,");
            streamClient.Write(data, 0, data.Length);

            data = MySerializer.serialize(gamesList[0]);
            streamClient.Write(data, 0, data.Length);

            data = new byte[1024];
            data1 = new byte[1024];

            // reading info about new game
            streamClient.Read(data);
            streamHost.Read(data1);
            Console.WriteLine("geted new games");

            Game myGame1 = (Game)MySerializer.deserialize(data);
            Game myGame2 = (Game)MySerializer.deserialize(data1);

            ((GameClassTTT)myGame1).turn(1, 1);

            streamClient.Write(MySerializer.serialize(myGame1));
            Console.WriteLine("game with new turn sended");

            data1 = new byte[1024];
            streamHost.Read(data1);
            Console.WriteLine("game with new turn recived!");
            myGame2 = (Game)MySerializer.deserialize(data1);

            Console.WriteLine(((GameClassTTT)myGame2).field[1, 1]);
        }
    }
}
