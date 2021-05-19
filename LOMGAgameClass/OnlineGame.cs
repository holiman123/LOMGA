using System;
using System.Net.Sockets;

namespace LOMGAgameClass
{
    [Serializable]
    public class OnlineGame
    {
        public NetworkStream hostStream;
        public NetworkStream clientStream;

        public bool isGameready;
        public bool isStarted;
    }
}
