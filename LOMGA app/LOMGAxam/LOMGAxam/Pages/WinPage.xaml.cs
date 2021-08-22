using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LOMGAgameClass;

namespace LOMGAxam.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WinPage : ContentPage
    {
        public WinPage(Account winAcc)
        {
            InitializeComponent();

            label.Text = winAcc.nickname + " won!";
        }

        protected override void OnAppearing()
        {
            allPage.FadeTo(1, App.fadingTimeConst);
            base.OnAppearing();
        }

        private void Button_Pressed(object sender, EventArgs e)
        {
            allPage.FadeTo(0, App.fadingTimeConst);
            Navigation.PopToRootAsync(false);
        }
    }
}