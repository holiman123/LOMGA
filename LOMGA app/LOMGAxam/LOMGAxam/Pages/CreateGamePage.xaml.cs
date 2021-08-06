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
    public partial class CreateGamePage : ContentPage
    {
        public CreateGamePage()
        {
            InitializeComponent();
        }

        private void Back_Button_Pressed(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}