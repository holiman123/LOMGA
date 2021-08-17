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

        private void ServerBrowse_Button_Pressed(object sender, EventArgs e)
        {
            allPage.FadeTo(0, App.fadingTimeConst);
            Navigation.PushAsync(App.listPage, false);
        }

        async private void Back_Button_Pressed(object sender, EventArgs e)
        {
            await allPage.FadeTo(0, App.fadingTimeConst);
            await Navigation.PopAsync(false);
        }

        async private void Button_FastStart_Pressed(object sender, EventArgs e)
        {
            await allPage.FadeTo(0, App.fadingTimeConst);
            await Navigation.PushAsync(App.fastGameSettingsPage, false);
        }

        async protected override void OnAppearing()
        {
            await allPage.FadeTo(1, App.fadingTimeConst);
            base.OnAppearing();
        }
    }
}