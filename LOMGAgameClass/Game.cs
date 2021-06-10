using System;
using System.Net.Sockets;

namespace LOMGAgameClass
{
    [Serializable]
    public class Game
    {
        public int gameType;
        public int gameIndex;
        public bool isGameready;
        public bool isStarted;

        public static bool operator ==(Game a, Game b) =>
            a.gameIndex == b.gameIndex;

        public static bool operator !=(Game a, Game b) =>
            a.gameIndex != b.gameIndex;
    }
}
