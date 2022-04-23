using System;
using System.Collections.Generic;
using System.Text;

namespace CurtainMonitor.Services
{
    public interface IStatusChanged
    {
        void OnControllerStatusChanged();
    }
}
