using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace LOMGAgameClass
{
    public class OnlineGame
    {
        public Game gameExemplar;
        public NetworkStream hostStream;
        public NetworkStream clientStream;

        public OnlineGame(Game gameExemplar, NetworkStream hostStream, NetworkStream clientStream)
        {
            this.gameExemplar = gameExemplar;
            this.clientStream = clientStream;
            this.hostStream = hostStream;
        }
        public OnlineGame() { }
    }
}
