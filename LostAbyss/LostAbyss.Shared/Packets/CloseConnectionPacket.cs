﻿using LostAbyss.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAbyss.Shared.Packets
{
    [Packet(0x03)]
    public class CloseConnectionPacket : BasePacket
    {
        [Field(0)]
        public string Reason;
    }
}
