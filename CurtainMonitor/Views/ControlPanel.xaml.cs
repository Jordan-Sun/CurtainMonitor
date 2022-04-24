using System.Diagnostics;
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
            if (BindingContext is ViewModels.ControlPanelModel model)
            {
                CurtainRaiseButton.Pressed += model.OnCurtainRaisePressed;
                CurtainRaiseButton.Released += model.OnCurtainReleased;
                CurtainLowerButton.Pressed += model.OnCurtainLowerPressed;
                CurtainLowerButton.Released += model.OnCurtainReleased;
            }
            else
            {
                Debug.WriteLine("Binding context is not control panel model");
            }
        }
    }
}