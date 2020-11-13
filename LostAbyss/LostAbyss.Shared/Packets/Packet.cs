using LostAbyss.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LostAbyss.Shared.Packets
{
    public class Packet // Just to ensure all packets can pass through the same arg
    {
        private MemoryStream data;
        public byte id;

        public Packet()
        {
            this.data = new MemoryStream();
            var attr = this.GetType().GetCustomAttribute<PacketAttribute>();
            if (attr is not null)
                this.id = attr.Id;
        }

        public static Packet ReadFromStream(Stream s)
        {
            BinaryReader bin = new(s);

            var id = bin.ReadByte();

            if(id == 0x0)
                return null;

            var packettype = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsSubclassOf(typeof(Packet))).First(x => x.GetCustomAttribute<PacketAttribute>().Id == id);
            var length = bin.ReadInt32();
            var data = new MemoryStream(bin.ReadBytes(length));

            var packet = (Packet)Activator.CreateInstance(packettype);
            packet.id = id;
            packet.data = data;
            packet.Populate();

            return packet;
            // read a populated packet from stream.
        }

        public void WriteToStream(Stream s)
        {
            BinaryWriter bin = new(s);

            this.Depopulate();

            bin.Write(this.id);
            bin.Write(this.data.Length);
            this.data.CopyTo(s);
        }

        private void Populate()
        {
            var fields = this.GetType().GetFields()
                .Where(x => x.GetCustomAttribute<FieldAttribute>() != null)
                .OrderBy(x => x.GetCustomAttribute<FieldAttribute>().Order);

            var bin = new BinaryReader(this.data);

            foreach (var f in fields)
            {
                object value = null;

                if (f.FieldType == typeof(string))
                    value = bin.ReadString();
                else if (f.FieldType == typeof(int))
                    value = bin.ReadInt32();
                else
                    throw new Exception($"Invalid field type: {f.FieldType.Name} on field {f.Name} in {this.GetType().Name}!");

                f.SetValue(this, value);
            }
        }

        private void Depopulate()
        {
            var fields = this.GetType().GetFields()
                .Where(x => x.GetCustomAttribute<FieldAttribute>() != null)
                .OrderBy(x => x.GetCustomAttribute<FieldAttribute>().Order);

            var bin = new BinaryWriter(this.data);

            foreach (var f in fields)
            {
                object value = f.GetValue(this);

                if (f.FieldType == typeof(string))
                    bin.Write((string)value);
                else if (f.FieldType == typeof(int))
                    bin.Write((int)value);
                else
                    throw new Exception($"Invalid field type: {f.FieldType.Name} on field {f.Name} in {this.GetType().Name}!");

                f.SetValue(this, value);
            }
        }
    }
}
