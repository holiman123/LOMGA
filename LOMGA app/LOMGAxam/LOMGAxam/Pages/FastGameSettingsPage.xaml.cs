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
    public partial class FastGameSettingsPage : ContentPage
    {
        bool TTTCheckBox = false;
        public FastGameSettingsPage()
        {
            InitializeComponent();
        }

        async private void Back_Button_Pressed(object sender, EventArgs e)
        {
            await allPage.FadeTo(0, App.fadingTimeConst);
            await Navigation.PopAsync(false);
        }

        async private void CheckBox_TTT(object sender, EventArgs e)
        {
            TTTCheckBox = !TTTCheckBox;
            if (TTTCheckBox)
                await TTT_tick.FadeTo(1, 150);
            else
                await TTT_tick.FadeTo(0, 150);
        }

        async protected override void OnAppearing()
        {
            await allPage.FadeTo(1, App.fadingTimeConst);
            base.OnAppearing();
        }
    }
}