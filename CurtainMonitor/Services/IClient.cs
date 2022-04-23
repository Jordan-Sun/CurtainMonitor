using System;
using System.Collections.Generic;
using System.Text;

namespace CurtainMonitor.Services
{
    public interface IClient
    {
        bool IsConnected
        {
            get;
        }
        bool Connect(String server, int port = 0);
    }
}
