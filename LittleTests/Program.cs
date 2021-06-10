using System;
using System.Collections.Generic;
using System.Threading;
using LOMGAgameClass;
using LOMGAserver;

namespace LittleTests
{
    class Program
    {

        static void Main(string[] args)
        {
            GameClassTTT c1 = new GameClassTTT();
            c1.gameIndex = 3;
            Game g1 = c1;
            byte[] data = MySerializer.serialize(c1);
            Game g2 = (Game)MySerializer.deserialize(data);
            GameClassTTT c2 = (GameClassTTT)g2;
            Console.WriteLine(c2.gameIndex);

            Console.ReadKey();
        }
    }
}
