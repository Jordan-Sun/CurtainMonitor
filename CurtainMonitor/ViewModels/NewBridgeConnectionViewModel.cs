using CurtainMonitor.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace CurtainMonitor.ViewModels
{
    [QueryProperty("ClientId", "ClientId")]
    public class NewConnectionViewModel : BaseViewModel
    {
        private string host;
        private string port;

        private IClient client;
        private string clientId;
        public string ClientId
        {
            get
            {
                return clientId;
            }
            set
            {
                clientId = value;
                client = Controller.GetClient(value);
            }
        }

        public NewConnectionViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(host)
                && Int32.TryParse(port, out _);
        }
        
        public string Text
        {
            get => host;
            set => SetProperty(ref host, value);
        }

        public string Description
        {
            get => port;
            set => SetProperty(ref port, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        
        private async void OnSave()
        {
            client.Connect(host, Int32.Parse(port));
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
