using System;
using System.Collections.Generic;
using System.Threading;
using System.Text.Json;
using LOMGAgameClass;

namespace LittleTests
{
    class Program
    {
        public static List<OnlineGame> testList = new List<OnlineGame>();

        static void Main(string[] args)
        {
            testList.Add(new GameClassTTT());
            testList.Add(new GameClassTTT());
            Thread t = new Thread(new ParameterizedThreadStart(ser));
            t.Start();

            Console.ReadKey();
        }

        public static void ser(object s)
        {
            Console.WriteLine(JsonSerializer.Serialize(testList));
        }
    }
}
