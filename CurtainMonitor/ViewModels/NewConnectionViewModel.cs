using CurtainMonitor.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace CurtainMonitor.ViewModels
{
    public class NewBridgeConnectionViewModel : BaseViewModel
    {
        private string host;
        private string user;

        private LightHTMLClient client;

        public NewBridgeConnectionViewModel()
        {
            client = Controller.Light;
            SaveCommand = new Command(OnSave, ValidateSave);
            NewUserCommand = new Command(OnNewUser, ValidateNewUser);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            this.PropertyChanged +=
                (_, __) => NewUserCommand.ChangeCanExecute();
        }

        private bool ValidateNewUser()
        {
            return !String.IsNullOrWhiteSpace(host);
        }
        
        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(host) && !String.IsNullOrWhiteSpace(user);
        }
        
        public string Text
        {
            get => host;
            set => SetProperty(ref host, value);
        }
        public string Description
        {
            get => user;
            set => SetProperty(ref user, value);
        }

        public Command SaveCommand { get; }
        public Command NewUserCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnNewUser()
        {
            client.Connect(host);
            string response = await client.NewUser();
            if (response.Equals("Link Button Not Pressed"))
            {
                await Shell.Current.DisplayAlert("Error", "Please press the link button and try again.", "OK");
            }
            else
            {
                Description = response;
            }
        }

        private async void OnSave()
        {
            client.Connect(host);
            client.Username = Description;
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
