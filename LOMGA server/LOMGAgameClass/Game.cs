using System;
using System.Collections.Generic;

namespace LOMGAgameClass
{
    [Serializable]
    public class Game
    {
        public List<Account> accounts = new List<Account>();
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
