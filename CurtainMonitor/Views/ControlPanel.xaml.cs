using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CurtainMonitor.Views
{
    public partial class ControlPanel : ContentPage
    {
        public ControlPanel()
        {
            InitializeComponent();
        }
        private void CurtainReleased(object sender, EventArgs e)
        {
            Console.WriteLine("Stopped toggling curtain");
        }
    }
}