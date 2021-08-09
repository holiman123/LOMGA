using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOMGAgameClass;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LOMGAxam.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TTTgamePage : ContentPage
    {
        public TTTgamePage()
        {
            InitializeComponent();
        }

        private void Turn_Button_Pressed(object sender, EventArgs e)
        {
            ((GameClassTTT)App.currentGame).turn(1, 1);
            byte[] data = MySerializer.serialize(App.currentGame);
            App.stream.Write(data, 0, data.Length);
        }

        protected override void OnAppearing()
        {
            allPage.FadeTo(1, App.fadingTimeConst);
            base.OnAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
            //-----------------------------------
            App.connectionThread.Abort();           //      ---TODO---
            Navigation.PopToRootAsync(false);       //  This section MUST be in 'Yes' button for leaving 'Pressed'
            //-----------------------------------
            return true;
        }
    }
}