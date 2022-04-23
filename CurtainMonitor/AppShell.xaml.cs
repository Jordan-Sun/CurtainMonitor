using CurtainMonitor.ViewModels;
using CurtainMonitor.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CurtainMonitor
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(NewConnectionPage), typeof(NewConnectionPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
