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
    public abstract class BasePacket // Just to ensure all packets can pass through the same arg
    {
        public BasePacket()
        {
        }

        public byte GetPacketId()
        {
            return this.GetType().GetCustomAttribute<PacketAttribute>().Id;
        }

        public void PopulateFromByteArray(byte[] input)
        {
            var fields = this.GetType().GetFields()
                .Where(x => x.GetCustomAttribute<FieldAttribute>() != null)
                .OrderBy(x => x.GetCustomAttribute<FieldAttribute>().Order);

            MemoryStream ms = new MemoryStream(input);
            BinaryReader br = new BinaryReader(ms);

            foreach (var f in fields)
            {
                object value = null;
                if (f.FieldType == typeof(string))
                    value = br.ReadString();
                else if (f.FieldType == typeof(int))
                    value = br.ReadInt32();
                else if (f.FieldType == typeof(long))
                    value = br.ReadInt64();
                else if (f.FieldType == typeof(short))
                    value = br.ReadInt16();
                else if (f.FieldType == typeof(uint))
                    value = br.ReadUInt32();
                else if (f.FieldType == typeof(ulong))
                    value = br.ReadUInt64();
                else if (f.FieldType == typeof(ushort))
                    value = br.ReadUInt16();
                else if (f.FieldType == typeof(bool))
                    value = br.ReadBoolean();
                else if (f.FieldType == typeof(sbyte))
                    value = br.ReadSByte();
                else if (f.FieldType == typeof(byte))
                    value = br.ReadByte();
                else if (f.FieldType == typeof(byte[]))
                {
                    var length = br.ReadInt32();
                    value = br.ReadBytes(length);
                }

                f.SetValue(this, null);
            }
        }

        public byte[] DepopulateToByteArray()
        {
            var fields = this.GetType().GetFields()
                .Where(x => x.GetCustomAttribute<FieldAttribute>() != null)
                .OrderBy(x => x.GetCustomAttribute<FieldAttribute>().Order);

            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);

            foreach (var f in fields)
            {
                object value = f.GetValue(this);

                if (f.FieldType == typeof(string))
                    br.Write((string)value);
                else if (f.FieldType == typeof(int))
                    br.Write((int)value);
                else if (f.FieldType == typeof(long))
                    br.Write((long)value);
                else if (f.FieldType == typeof(short))
                    br.Write((short)value);
                else if (f.FieldType == typeof(uint))
                    br.Write((uint)value);
                else if (f.FieldType == typeof(ulong))
                    br.Write((ulong)value);
                else if (f.FieldType == typeof(ushort))
                    br.Write((ushort)value);
                else if (f.FieldType == typeof(bool))
                    br.Write((bool)value);
                else if (f.FieldType == typeof(sbyte))
                    br.Write((sbyte)value);
                else if (f.FieldType == typeof(byte))
                    br.Write((byte)value);
                else if (f.FieldType == typeof(byte[]))
                {
                    br.Write(((byte[])value).Length);
                    br.Write((byte[])value);
                }
                else { /*write nothing*/ }
            }

            return ms.ToArray();
        }
    }
}
