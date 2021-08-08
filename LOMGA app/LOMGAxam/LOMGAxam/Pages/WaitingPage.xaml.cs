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
    public partial class WaitingPage : ContentPage
    {
        bool animFlag = false;
        public WaitingPage()
        {
            InitializeComponent();
        }

        async protected override void OnAppearing()
        {
            animFlag = true;
            await allPage.FadeTo(1, App.fadingTimeConst);
            base.OnAppearing();

            while (animFlag)
            {
                await Task.Delay(800);
                Dots.Text = "...";
                await Task.Delay(800);
                Dots.Text = " ..";
                await Task.Delay(800);
                Dots.Text = "  .";
                await Task.Delay(800);
                Dots.Text = "   ";
                await Task.Delay(800);
                Dots.Text = ".  ";
                await Task.Delay(800);
                Dots.Text = ".. ";
            }
        }
        protected override void OnDisappearing()
        {
            Dots.Text = "   ";
            animFlag = false;
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}