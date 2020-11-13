using LostAbyss.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAbyss.Shared.Packets
{
    [Packet(0x02)]
    public class RequestServerStatusPacket : Packet
    {
        // No data, just a request
    }
}
