using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOMGAxam.Pages;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LOMGAxam
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void Button_Play_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(App.chooseConnectScreen);
        }

        private void Button_CreateGame_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(App.createGamePage);
        }
    }
}