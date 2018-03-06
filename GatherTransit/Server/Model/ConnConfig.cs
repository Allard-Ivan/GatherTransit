using System;
using System.Collections.Generic;
using System.Text;

namespace GatherTransit.Server.Model
{
    class ConnConfig
    {
        private int port;
        private int numConnections;
        private int receiveBufferSize;
        private int overtime;
        private int absoluted;

        public int Port { get => port; set => port = value; }
        public int NumConnections { get => numConnections; set => numConnections = value; }
        public int ReceiveBufferSize { get => receiveBufferSize; set => receiveBufferSize = value; }
        public int Overtime { get => overtime; set => overtime = value; }
        public int Absoluted { get => absoluted; set => absoluted = value; }
    }
}
