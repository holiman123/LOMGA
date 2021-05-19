using System;
using System.Net.Sockets;

namespace LOMGAgameClass
{
    [Serializable]
    public class OnlineGame
    {
        public int gameIndex;
        public NetworkStream hostStream;
        public NetworkStream clientStream;

        public bool isGameready;
        public bool isStarted;

        public static bool operator ==(OnlineGame a, OnlineGame b) =>
            a.gameIndex == b.gameIndex;

        public static bool operator !=(OnlineGame a, OnlineGame b) =>
            a.gameIndex != b.gameIndex;
    }
}
