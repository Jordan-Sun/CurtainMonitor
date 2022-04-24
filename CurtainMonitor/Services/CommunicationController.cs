using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace CurtainMonitor.Services
{
    public class CommunicationController : ISensorController, IStatusChanged
    {
        public SensorTcpClient Indoor;
        public SensorTcpClient Outdoor;
        public CurtainTcpClient Curtain;
        public LightHTMLClient Light;

        public IStatusChanged Recipient;

        private int dimThreshold;
        public int DimThreshold
        {
            set
            {
                dimThreshold = value;
                OnNewData();
            }
            get { return dimThreshold; }
        }
        private int brightThreshold;
        public int BrightThreshold
        {
            set
            {
                brightThreshold = value;
                OnNewData();
            }
            get { return brightThreshold; }
        }

        public bool CanAutoControl
        {
            get {  return Indoor.IsConnected && Outdoor.IsConnected && Curtain.IsConnected; }
        }
        public bool ManualMode;
        public bool AutoControl
        {
            get { return CanAutoControl && !ManualMode; }
        }
        
        public CommunicationController()
        {
            Indoor = new SensorTcpClient(this);
            Indoor.Recipient = this;
            Outdoor = new SensorTcpClient(this);
            Outdoor.Recipient = this;
            Curtain = new CurtainTcpClient();
            Curtain.Recipient = this;
            Light = new LightHTMLClient();
            Light.Recipient = this;
            dimThreshold = 300;
            brightThreshold = 500;
            ManualMode = false;
        }
        private void DoAutoControl()
        {
            Debug.WriteLine("Auto Controlling with Dim Threshold: " + DimThreshold + " Bright Threshold: " + BrightThreshold + " Indoor Lux: " + Indoor.Visible + " Outdoor Lux: " + Outdoor.Visible);
            if (Indoor.Visible <= DimThreshold)
            {
                /* Dim Indoor */
                if (Outdoor.Visible <= DimThreshold)
                {
                    /* Dim Outdoor */
                    Curtain.Stop();
                }
                else
                {
                    /* Comfort or Bright Outdoor */
                    Curtain.Raise();
                }
            }
            else if (Indoor.Visible > BrightThreshold)
            {
                /* Bright Indoor */
                if (Outdoor.Visible <= DimThreshold)
                {
                    /* Dim Outdoor */
                    Curtain.Stop();
                }
                else
                {
                    /* Comfort or Bright Outdoor */
                    Curtain.Lower();
                }
            }
            else
            {
                /* Comfort Indoor */
                Curtain.Stop();
            }
        }
        public void OnNewData()
        {
            if (AutoControl)
            {
                DoAutoControl();
            }
        }

        public IClient GetClient(string id)
        {
            switch (id)
            {
                case "Indoor":
                    return Indoor;
                case "Outdoor":
                    return Outdoor;
                case "Curtain":
                    return Curtain;
                case "Light":
                    return Light;
                default:
                    return null;
            }
        }

        public void OnControllerStatusChanged()
        {
            Recipient?.OnControllerStatusChanged();
        }
    }
}    
