using System;
using System.Collections.Generic;
using System.Text;

namespace LostAbyss.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PacketAttribute : Attribute
    {
        public byte Id { get; private set; }

        public PacketAttribute(byte id) : base()
        {
            this.Id = id;
        }
    }
}
