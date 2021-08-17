using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LOMGAgameClass;

namespace LOMGAxam.Pages
{
    public class StringFromGame
    {
        public string type { get; set; }

        public StringFromGame(string type)
        {
            this.type = type;
        }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : ContentPage
    {
        public static List<Game> recivedGames = new List<Game>();
        public ObservableCollection<StringFromGame> stringsFromGames { get; set; }

        public ListPage()
        {
            InitializeComponent();

            stringsFromGames = new ObservableCollection<StringFromGame>();

            list_Refreshing(new object(), new EventArgs());

            this.BindingContext = this;
        }

        async private void Back_Button_Pressed(object sender, EventArgs e)
        {
            await allPage.FadeTo(0, App.fadingTimeConst);
            await Navigation.PopAsync(false);
        }

        async protected override void OnAppearing()
        {
            await allPage.FadeTo(1, App.fadingTimeConst);
            base.OnAppearing();
        }

        private void list_Refreshing(object sender, EventArgs e)
        {
            recivedGames.Add(new Game());   // Getting list from server

            stringsFromGames.Clear();

            foreach (Game game in recivedGames)
            {
                switch (game.gameType)
                {
                    case 0:
                    stringsFromGames.Add(new StringFromGame("TTT"));
                        break;
                }
            }
            list.IsRefreshing = false;
        }
    }
}