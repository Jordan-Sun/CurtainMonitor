using CurtainMonitor.Services;
using CurtainMonitor.Views;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Diagnostics;

namespace CurtainMonitor.ViewModels
{
    public class ControlPanelModel : BaseViewModel, IStatusChanged
    {
        /* Indoor Properties */
        private bool indoorConnected;
        public bool IndoorConnected
        {
            private set
            {
                SetProperty(ref indoorConnected, value);
                IndoorIndicationLight = value ? Color.Green : Color.Red;
                IndoorIndicationText = value ? "Connected" : "Disconnected";
            }
            get
            {
                return indoorConnected;
            }
        }
        private Color indoorIndicationLight;
        public Color IndoorIndicationLight
        {
            private set
            {
                SetProperty(ref indoorIndicationLight, value);
            }
            get
            {
                return indoorIndicationLight;
            }
        }
        private String indoorIndicationText;
        public String IndoorIndicationText
        {
            private set
            {
                SetProperty(ref indoorIndicationText, value);
            }
            get
            {
                return indoorIndicationText;
            }
        }
        
        /* Indoor Functions */
        private async void ConnectIndoor()
        {
            await Shell.Current.GoToAsync($"{nameof(NewConnectionPage)}?{nameof(NewConnectionViewModel.ClientId)}=Indoor");
        }

        /* Outdoor Properties */
        private bool outdoorConnected;
        public bool OutdoorConnected
        {
            private set
            {
                SetProperty(ref outdoorConnected, value);
                OutdoorIndicationLight = value ? Color.Green : Color.Red;
                OutdoorIndicationText = value ? "Connected" : "Disconnected";
            }
            get
            {
                return outdoorConnected;
            }
        }
        private Color outdoorIndicationLight;
        public Color OutdoorIndicationLight
        {
            private set
            {
                SetProperty(ref outdoorIndicationLight, value);
            }
            get
            {
                return outdoorIndicationLight;
            }
        }
        private String outdoorIndicationText;
        public String OutdoorIndicationText
        {
            private set
            {
                SetProperty(ref outdoorIndicationText, value);
            }
            get
            {
                return outdoorIndicationText;
            }
        }

        /* Outdoor Functions */
        private async void ConnectOutdoor()
        {
            await Shell.Current.GoToAsync($"{nameof(NewConnectionPage)}?{nameof(NewConnectionViewModel.ClientId)}=Outdoor");
        }

        /* Curtain Properties */
        private bool curtainConnected;
        public bool CurtainConnected
        {
            private set
            {
                SetProperty(ref curtainConnected, value);
                CurtainIndicationLight = value ? Color.Green : Color.Red;
                CurtainIndicationText = value ? "Connected" : "Disconnected";
            }
            get
            {
                return curtainConnected;
            }
        }
        private Color curtainIndicationLight;
        public Color CurtainIndicationLight
        {
            private set
            {
                SetProperty(ref curtainIndicationLight, value);
            }
            get
            {
                return curtainIndicationLight;
            }
        }
        private String curtainIndicationText;
        public String CurtainIndicationText
        {
            private set
            {
                SetProperty(ref curtainIndicationText, value);
            }
            get
            {
                return curtainIndicationText;
            }
        }

        /* Curtain Functions */
        private async void ConnectCurtain()
        {
            await Shell.Current.GoToAsync($"{nameof(NewConnectionPage)}?{nameof(NewConnectionViewModel.ClientId)}=Curtain");
        }
        private void ToggleCurtain(string direction)
        {
            AutoMode = false;
            switch (direction)
            {
                case "Raise":
                    Controller.Curtain.Raise();
                    break;
                case "Lower":
                    Controller.Curtain.Lower();
                    break;
                case "Stop":
                    Controller.Curtain.Stop();
                    break;
                default:
                    break;
            }
            Debug.WriteLine("Started toggling curtain in the " + direction + " direction");
        }
        public void OnCurtainRaisePressed(object sender, EventArgs e)
        {
            ToggleCurtain("Raise");
        }
        public void OnCurtainLowerPressed(object sender, EventArgs e)
        {
            ToggleCurtain("Lower");
        }
        public void OnCurtainReleased(object sender, EventArgs e)
        {
            ToggleCurtain("Stop");
        }

        /* Light Properties */
        private bool lightConnected;
        public bool LightConnected
        {
            private set {
                SetProperty(ref lightConnected, value);
                LightIndicationLight = value ? Color.Green : Color.Red;
                LightIndicationText = value ? "Connected" : "Disconnected";
                (LightCommand as Command).ChangeCanExecute();
            }
            get {
                return lightConnected;
            }
        }
        private Color lightIndicationLight;
        public Color LightIndicationLight
        {
            private set
            {
                SetProperty(ref lightIndicationLight, value);
            }
            get
            {
                return lightIndicationLight;
            }
        }
        private String lightIndicationText;
        public String LightIndicationText
        {
            private set
            {
                SetProperty(ref lightIndicationText, value);
            }
            get
            {
                return lightIndicationText;
            }
        }
        public ICommand LightCommand { get; }

        /* Light Functions */
        private async void ConnectLight()
        {
            await Shell.Current.GoToAsync($"{nameof(NewBridgeConnectionPage)}");
        }
        private async void ToggleLight()
        {
            AutoMode = false;
            await Controller.Light.Toggle();
        }

        public void OnControllerStatusChanged()
        {
            IndoorConnected = Controller.Indoor.IsConnected;
            OutdoorConnected = Controller.Outdoor.IsConnected;
            CurtainConnected = Controller.Curtain.IsConnected;
            LightConnected = Controller.Light.IsConnected;
            CanAutoControl = Controller.CanAutoControl;
        }

        /* Auto/Manual Control Properties */
        public bool AutoMode
        {
            set
            {
                SetProperty(ref Controller.ManualMode, !value);
                AutoModeText = value ? "Auto" : "Manual";
            }
            get
            {
                return !Controller.ManualMode;
            }
        }

        private bool canAutoControl;
        public bool CanAutoControl
        {
            private set
            {
                SetProperty(ref canAutoControl, value);
            }
            get
            {
                return canAutoControl;
            }
        }
        private string autoModeText;
        public string AutoModeText
        {
            private set
            {
                SetProperty(ref autoModeText, value);
            }
            get
            {
                return autoModeText;
            }
        }

        /* Dim/Bright Threshold Properties */
        private int dimThreshold;
        public int DimThreshold
        {
            get
            {
                return dimThreshold;
            }
            set
            {
                Controller.DimThreshold = value;
                SetProperty(ref dimThreshold, value);
            }
        }
        private int brightThreshold;
        public int BrightThreshold
        {
            get
            {
                return brightThreshold;
            }
            set
            {
                Controller.BrightThreshold = value;
                SetProperty(ref brightThreshold, value);
            }
        }
        public ICommand ConnectCommand { get; }

        /* Main Model */
        public ControlPanelModel()
        {
            Title = "Control Panel";

            ConnectCommand = new Command<string>((type) =>
            {
                switch (type)
                {
                    case "Indoor":
                        ConnectIndoor();
                        break;
                    case "Outdoor":
                        ConnectOutdoor();
                        break;
                    case "Curtain":
                        ConnectCurtain();
                        break;
                    case "Light":
                        ConnectLight();
                        break;
                    default:
                        // this type of connection is not supported.
                        throw new NotSupportedException();
                }
            });
            LightCommand = new Command(execute: () =>
            {
                ToggleLight();
            }
            , canExecute: () =>
            {
                return LightConnected;
            });

            AutoMode = false;
            DimThreshold = 300;
            BrightThreshold = 500;
            OnControllerStatusChanged();

            Controller.Recipient = this;
        }


    }
}