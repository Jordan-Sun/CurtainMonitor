using System;

namespace CurtainMonitor.Models
{
    [Serializable]
    public struct Timespec
    {
        public int tv_sec;
        public int tv_nsec;

        public static Timespec Current()
        {
            Timespec ts = new Timespec();
            ts.tv_sec = (int)(DateTime.Now.Ticks / 10000000);
            ts.tv_nsec = (int)(DateTime.Now.Ticks % 10000000 * 100);
            return ts;
        }
        
        public static Timespec operator -(Timespec lhs, Timespec rhs)
        {
            Timespec ret = new Timespec();
            ret.tv_sec = lhs.tv_sec - rhs.tv_sec;
            ret.tv_nsec = lhs.tv_nsec - rhs.tv_nsec;
            if (ret.tv_nsec < 0)
            {
                ret.tv_sec--;
                ret.tv_nsec += 1000000000;
            }
            return ret;
        }
    }
}
