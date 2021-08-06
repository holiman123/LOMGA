using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LOMGAxam.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChooseConnectScreen : ContentPage
    {
        public ChooseConnectScreen()
        {
            InitializeComponent();
        }

        private void Back_Button_Pressed(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void Button_FastStart_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(App.fastGameSettingsPage);
        }
    }
}