using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Diagnostics;

namespace CurtainMonitor.ViewModels
{
    public class ControlPanelModel : BaseViewModel
    {
        private bool indoorConnected = false;
        private bool outdoorConnected = false;
        private bool curtainConnected = false;
        private bool lightConnected = false;
        
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
            CurtainCommand = new Command<string>(execute: (direction) =>
            {
                ToggleCurtain(direction);
            }
            , canExecute: (_) =>
            {
                return curtainConnected;
            });
            LightCommand = new Command(execute: () =>
            {
                ToggleLight();
            }
            , canExecute: () =>
            {
                return lightConnected;
            });
        }
        public ICommand ConnectCommand { get; }
        public ICommand CurtainCommand { get; }
        public ICommand LightCommand { get; }
        private void ConnectIndoor()
        {
            ConnectSensor("Indoor");
        }

        private void ConnectOutdoor()
        {
            ConnectSensor("Outdoor");
        }

        private void ConnectSensor(string type)
        {
            Debug.WriteLine(type + " Sensor");
        }

        private void ConnectCurtain()
        {
            curtainConnected = true;
            Debug.WriteLine("Connected to curtain");
        }
        private void ToggleCurtain(string direction)
        {
            Debug.WriteLine("Started toggling curtain in the " + direction + " direction");
        }

        private void ConnectLight()
        {
            lightConnected = true;
            Debug.WriteLine("Connected to light");
        }

        private void ToggleLight()
        {
            Debug.WriteLine("Toggled light");
        }

    }
}