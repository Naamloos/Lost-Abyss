using LostAbyss.Shared.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LostAbyss.Server
{
    public class Client
    {
        private TcpClient _tcp;
        private bool _loggedin = false;
        private Queue<Packet> _packetQueue = new Queue<Packet>();

        public Client(TcpClient tcp)
        {
            this._tcp = tcp;
        }

        /// <summary>
        /// Ticks client. Runs parallel from other clients.
        /// </summary>
        /// <returns></returns>
        public async Task TickAsync()
        {
            var str = _tcp.GetStream();

            if (_packetQueue.TryDequeue(out Packet packet))
            {
                packet.WriteToStream(str);
            }

            var pack = Packet.ReadFromStream(str);
            if (pack is not null)
                HandlePacket(pack);


            await Task.Yield();
        }

        public void EnqueuePacket(Packet packet)
        {
            this._packetQueue.Enqueue(packet);
        }

        public bool IsLoggedIn() => _loggedin;

        private void HandlePacket(Packet p)
        {
            switch (p)
            {
                case ServerStatusPacket packet:
                    Console.WriteLine($"For some weird reason client sent server status?");
                    break;

                case RequestServerStatusPacket packet:
                    Console.WriteLine("Received server status request");
                    this.EnqueuePacket(new ServerStatusPacket()
                    {
                        ServerName = "InDev server",
                        ServerDesc = "A server that is still in dev.",
                        MaxPlayers = 69,
                        OnlinePlayers = 0
                    });
                    break;

                default:
                    Console.WriteLine($"Read unknown or null packet.");
                    break;
            }
        }
    }
}
