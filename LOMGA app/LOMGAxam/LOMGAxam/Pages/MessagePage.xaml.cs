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
    public partial class MessagePage : ContentPage
    {
        public MessagePage(string messageText)
        {
            InitializeComponent();

            label.Text = messageText;
        }

        private void Button_Pressed(object sender, EventArgs e)
        {
            allPage.FadeTo(0, App.fadingTimeConst);
            Navigation.PopToRootAsync(false);
        }

        async protected override void OnAppearing()
        {
            await allPage.FadeTo(1, App.fadingTimeConst);
            base.OnAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}