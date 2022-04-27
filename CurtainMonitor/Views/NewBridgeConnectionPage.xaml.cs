using CurtainMonitor.Models;
using CurtainMonitor.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CurtainMonitor.Views
{
    public partial class NewBridgeConnectionPage : ContentPage
    {
        public NewBridgeConnectionPage()
        {
            InitializeComponent();
            BindingContext = new NewBridgeConnectionViewModel();
        }
    }
}