using System;

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
    }
}
