using System;
using System.IO;
using System.Text;

namespace CurtainMonitor.Models
{
    public struct CurtainPacket
    {
        /* Metadata */
        public Timespec timestamp;
        public uint sequence;
        /* Data */
        public int direction;
        /*
        when turns > 0, the motor will rotate turns rounds.
        when turns < 0, the motor will rotate indefinitely.
        when turns = 0, the motor will stop.
        */
        public int turns;

        public static CurtainPacket ReadPacket(Stream stream)
        {
            CurtainPacket packet = new CurtainPacket();
            BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true);
            packet.timestamp.tv_sec = reader.ReadInt32();
            packet.timestamp.tv_nsec = reader.ReadInt32();
            packet.sequence = reader.ReadUInt32();
            _ = reader.ReadInt32();
            packet.direction = reader.ReadInt32();
            packet.turns = reader.ReadInt32();
            return packet;
        }

        public static void SendPacket(Stream stream, CurtainPacket packet)
        {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, true);
            writer.Write(packet.timestamp.tv_sec);
            writer.Write(packet.timestamp.tv_nsec);
            writer.Write(packet.sequence);
            writer.Write(0);
            writer.Write(packet.direction);
            writer.Write(packet.turns);
        }
    }
}
