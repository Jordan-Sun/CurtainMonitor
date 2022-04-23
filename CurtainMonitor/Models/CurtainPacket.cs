using System;

namespace CurtainMonitor.Models
{
    [Serializable]
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
    }
}
