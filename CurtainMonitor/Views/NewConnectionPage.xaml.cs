using CurtainMonitor.Models;
using CurtainMonitor.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CurtainMonitor.Views
{
    public partial class NewConnectionPage : ContentPage
    {
        public Item Item { get; set; }

        public NewConnectionPage()
        {
            InitializeComponent();
            BindingContext = new NewConnectionViewModel();
        }
    }
}