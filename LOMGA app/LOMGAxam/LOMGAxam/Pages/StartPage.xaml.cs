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

        private async void Button_Play_Pressed(object sender, EventArgs e)
        {
            await allPage.FadeTo(0, App.fadingTimeConst);
            await Navigation.PushAsync(App.chooseConnectScreen, false);
        }

        private async void Button_CreateGame_Pressed(object sender, EventArgs e)
        {
            await allPage.FadeTo(0, App.fadingTimeConst);
            await Navigation.PushAsync(App.createGamePage, false);
        }

        async protected override void OnAppearing()
        {
            await allPage.FadeTo(1, App.fadingTimeConst);
            base.OnAppearing();
        }

        private void MyEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            App.nickname = ((MyEntry)sender).Text;

            if (((MyEntry)sender).Text != "")
            {
                CreateGameButton.FadeTo(1, 250);
                PlayButton.FadeTo(1, 250);
            }
            else
            {
                CreateGameButton.FadeTo(0, 250);
                PlayButton.FadeTo(0, 250);
            }
        }
    }
}