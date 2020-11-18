using LostAbyss.Shared.Attributes;
using LostAbyss.Shared.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LostAbyss.Shared
{
    public class Connection
    {
        private TcpClient _tcp;
        private CancellationToken _ct;
        private object _lock = new object();

        public delegate Task PacketReceivedAsyncDelegate(BasePacket p);
        public event PacketReceivedAsyncDelegate PacketReceivedAsync;

        public Connection(TcpClient tcp, CancellationToken ct)
        {
            this._tcp = tcp;
            this._ct = new CancellationToken();
        }

        public async Task StartClientLoopAsync()
        {
            var stream = _tcp.GetStream();
            var reader = new BinaryReader(stream);

            while (!this._ct.IsCancellationRequested && this._tcp.Connected)
            {
                
                    var length = reader.ReadInt32();

                    if (length > 0)
                    {
                        var id = reader.ReadByte();
                        Console.WriteLine($"Read a packet with length {length}!");
                        var data = reader.ReadBytes(length - 1);

                        await this.handlePacketAsync(id, data);
                    }
            }

            await Task.Yield();
        }

        public async Task WritePacketAsync(BasePacket pack)
        {
            lock (_lock)
            {
                var stream = _tcp.GetStream();
                var writer = new BinaryWriter(stream);
                var bytes = pack.DepopulateToByteArray();
                writer.Write(bytes.Length + 1);
                writer.Write(pack.GetPacketId());
                writer.Write(bytes);
            }

            await Task.Yield();
        }

        private async Task handlePacketAsync(byte id, byte[] data)
        {
            var packets = this.GetType().Assembly.GetTypes()
                .Where(x => x.IsAssignableTo(typeof(BasePacket)) && x.GetCustomAttribute<PacketAttribute>() != null);
            if(packets.Count() > 0)
            {
                var packettype = packets.FirstOrDefault(x => x.GetCustomAttribute<PacketAttribute>().Id == id);
                if(packettype != null)
                {
                    var pack = (BasePacket)Activator.CreateInstance(packettype);
                    pack.PopulateFromByteArray(data);
                    await this.PacketReceivedAsync.Invoke(pack);
                }
            }
        }
    }
}
