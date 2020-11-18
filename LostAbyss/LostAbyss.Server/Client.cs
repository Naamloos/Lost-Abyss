using LostAbyss.Shared;
using LostAbyss.Shared.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LostAbyss.Server
{
    public class Client
    {
        private Connection _connection;
        private CancellationTokenSource _cts;

        public Client(TcpClient tcp)
        {
            this._cts = new CancellationTokenSource();
            this._connection = new Connection(tcp, _cts.Token);

            this._connection.PacketReceivedAsync += HandlePacket;
        }

        private async Task HandlePacket(BasePacket p)
        {
            switch(p)
            {
                default:
                    Console.WriteLine("Forcibly closing a connection because an invalid packet was sent.");
                    await this.DisconnectAsync("Invalid packet sent");
                    break;

                case RequestServerStatusPacket packet:
                    Console.WriteLine("Got server status request.");
                    await this._connection.WritePacketAsync(new ServerStatusPacket()
                    {
                        ServerName = "debug",
                        ServerDesc = "a debug server",
                        MaxPlayers = 69,
                        OnlinePlayers = 0
                    });
                    break;

                case CloseConnectionPacket packet:
                    Console.WriteLine($"Closed a connection on request with reason: {packet.Reason}");
                    break;
            }
        }

        public async Task StartConnectionAsync()
        {
            await this._connection.StartClientLoopAsync();
        }

        public async Task DisconnectAsync(string reason)
        {
            await this._connection.WritePacketAsync(new CloseConnectionPacket()
            {
                Reason = reason
            });
            this._cts.Cancel();
        }
    }
}
