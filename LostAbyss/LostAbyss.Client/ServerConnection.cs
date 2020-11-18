using LostAbyss.Shared;
using LostAbyss.Shared.Packets;
using System;
using System.Collections.Generic;
using System.IO;
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
        private Connection _connection;
        private CancellationTokenSource _cts;
        private IPAddress _ip;
        private int _port;

        public ServerConnection(IPAddress ip, int port)
        {
            this._cts = new CancellationTokenSource();
            this._ip = ip;
            this._port = port;
        }

        public async Task StartConnectionAsync()
        {
            var tcp = new TcpClient();
            tcp.Connect(_ip, _port);

            _connection = new Connection(tcp, _cts.Token);
            _connection.PacketReceivedAsync += HandlePacket;
            await this._connection.StartClientLoopAsync();
        }

        private async Task HandlePacket(BasePacket p)
        {
            switch (p)
            {
                default:
                    break;
                case ServerStatusPacket packet:
                    Console.WriteLine("Got server status.");
                    break;
            }

            await Task.Yield();
        }

        public async Task RequestServerStatusAsync()
        {
            await this._connection.WritePacketAsync(new RequestServerStatusPacket());
        }
    }
}
