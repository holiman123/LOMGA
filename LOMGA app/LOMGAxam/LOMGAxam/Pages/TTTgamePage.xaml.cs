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
    public partial class TTTgamePage : ContentPage
    {
        public TTTgamePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }
    }
}