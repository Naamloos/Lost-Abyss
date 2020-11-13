using LostAbyss.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAbyss.Shared.Packets
{
    [Packet(0x01)]
    public class ServerStatusPacket : Packet
    {
        [Field(0)]
        public string ServerName;

        [Field(1)]
        public string ServerDesc;

        [Field(2)]
        public int OnlinePlayers;

        [Field(3)]
        public int MaxPlayers;
    }
}
