using LostAbyss.Shared.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LostAbyss.Client
{
    public class ServerConnection
    {
        private IPAddress _ip;
        private int _port;
        private TcpClient _client;
        private CancellationTokenSource _cts;
        private ServerStatusPacket _status;

        public ServerConnection(IPAddress ip, int port)
        {
            this._ip = ip;
            this._port = port;
            this._cts = new CancellationTokenSource();
            _client = new TcpClient();
        }

        public async Task StartConnectionAsync()
        {
            _client.Connect(this._ip, this._port);
            Console.WriteLine("Cnnected");
            var str = _client.GetStream();

            Console.WriteLine("Requesting status");
            new RequestServerStatusPacket().WriteToStream(str);

            while (!_cts.IsCancellationRequested)
            {
                Console.WriteLine("Getting data");
                var pack = Packet.ReadFromStream(str);
                HandlePacket(pack);
                await Task.Delay(50);
            }
        }

        private void HandlePacket(Packet p)
        {
            switch (p)
            {
                case ServerStatusPacket packet:
                    Console.WriteLine("Received server status.");
                    this._status = packet;
                    break;

                default:
                    Console.WriteLine($"Received null / unknown packet.");
                    break;
            }
        }
    }
}
