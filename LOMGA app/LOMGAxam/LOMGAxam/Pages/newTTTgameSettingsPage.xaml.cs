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
    public partial class newTTTgameSettingsPage : ContentPage
    {
        bool passwordBool = false;

        public newTTTgameSettingsPage()
        {
            InitializeComponent();
        }

        async private void Back_Button_Pressed(object sender, EventArgs e)
        {
            await allPage.FadeTo(0, App.fadingTimeConst);
            await Navigation.PopAsync(false);
        }
        async private void Start_Button_Pressed(object sender, EventArgs e)
        {
            App.connectionThread = new System.Threading.Thread(unused => App.connectionThreadMethod("start,0", Navigation));

            await allPage.FadeTo(0, App.fadingTimeConst);
            await Navigation.PushAsync(App.waitingPage, false);

            App.connectionThread.Start();
        }

        async private void CheckBox_TTT(object sender, EventArgs e)
        {
            passwordBool = !passwordBool;
            if (passwordBool)
            {
                Password_Entry.IsEnabled = true;
                await Password_tick.FadeTo(1, 150);
                await Password_Entry.FadeTo(1, 150);
            }
            else
            {
                Password_Entry.IsEnabled = false;
                await Password_tick.FadeTo(0, 150);
                await Password_Entry.FadeTo(0, 150);
            }
        }
        async protected override void OnAppearing()
        {
            await allPage.FadeTo(1, App.fadingTimeConst);
            base.OnAppearing();
        }
    }
}