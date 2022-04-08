using CurtainMonitor.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace CurtainMonitor.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}