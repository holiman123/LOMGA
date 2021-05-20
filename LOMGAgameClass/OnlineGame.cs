using System;
using System.Net.Sockets;

namespace LOMGAgameClass
{
    [Serializable]
    public class OnlineGame
    {
        public int gameIndex { get; set; }
        public NetworkStream hostStream { get; set; }
        public NetworkStream clientStream { get; set; }

        public bool isGameready { get; set; }
        public bool isStarted { get; set; }

        public static bool operator ==(OnlineGame a, OnlineGame b) =>
            a.gameIndex == b.gameIndex;

        public static bool operator !=(OnlineGame a, OnlineGame b) =>
            a.gameIndex != b.gameIndex;
    }
}
