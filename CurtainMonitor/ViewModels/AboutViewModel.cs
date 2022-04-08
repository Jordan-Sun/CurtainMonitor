using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CurtainMonitor.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebpageCommand = new Command<string>(async (url) => await Browser.OpenAsync(url));
        }
        public ICommand OpenWebpageCommand { get; }

    }
}