using System;
using System.IO;
using System.Text;

namespace CurtainMonitor.Models
{
    [Serializable]
    public struct SensorPacket
    {
        /* Metadata */
        public Timespec timestamp;
        public uint sequence;
        /* Data */
        public int full;
        public int infrared;
        public int visible;

        public static SensorPacket ReadPacket(Stream stream)
        {
            SensorPacket packet = new SensorPacket();
            BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true);
            packet.timestamp.tv_sec = reader.ReadInt32();
            packet.timestamp.tv_nsec = reader.ReadInt32();
            packet.sequence = reader.ReadUInt32();
            packet.full = reader.ReadInt32();
            packet.infrared = reader.ReadInt32();
            packet.visible = reader.ReadInt32();
            return packet;
        }

        public static void SendPacket(Stream stream, SensorPacket packet)
        {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, true);
            writer.Write(packet.timestamp.tv_sec);
            writer.Write(packet.timestamp.tv_nsec);
            writer.Write(packet.sequence);
            writer.Write(packet.full);
            writer.Write(packet.infrared);
            writer.Write(packet.visible);
        }
    }
}
