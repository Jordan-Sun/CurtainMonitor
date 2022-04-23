using CurtainMonitor.Services;
using CurtainMonitor.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CurtainMonitor
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            
            DependencyService.Register<CommunicationController>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
