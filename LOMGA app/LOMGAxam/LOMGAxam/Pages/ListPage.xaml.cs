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
        public string hostNickname { get; set; }

        public StringFromGame(string type, string hostNickname)
        {
            this.type = type;
            this.hostNickname = hostNickname;
        }
    }



    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : ContentPage
    {
        public static int choosedIndex = -1;

        static bool showTTT = true;
        static bool showSB = true;
        static bool showWithPassword = true;
        static bool showWithoutPassword = true;

        public static List<Game> recivedGames = new List<Game>();
        public ObservableCollection<StringFromGame> stringsFromGames { get; set; }

        public ListPage()
        {
            InitializeComponent();

            stringsFromGames = new ObservableCollection<StringFromGame>();

            this.BindingContext = this;
        }

        protected override bool OnBackButtonPressed()
        {
            allPage.FadeTo(0, App.fadingTimeConst);
            Navigation.PopAsync(false);
            return true;
        }

        private void Back_Button_Pressed(object sender, EventArgs e)
        {
            OnBackButtonPressed();
        }

        async protected override void OnAppearing()
        {
            list_Refreshing(new object(), new EventArgs());
            await allPage.FadeTo(1, App.fadingTimeConst);
            base.OnAppearing();
        }

        private void list_Refreshing(object sender, EventArgs e)
        {
            recivedGames.Clear();
            App.connectionThread = new System.Threading.Thread(unused => App.connectionThreadMethod("list,"));
            App.connectionThread.Start();

            App.connectionThread.Join();

            stringsFromGames.Clear();

            foreach (Game game in recivedGames)
            {
                switch (game.gameType)
                {
                    case 1:
                    stringsFromGames.Add(new StringFromGame("TTT", game.accounts[0].nickname));
                        break;
                }
            }
            list.IsRefreshing = false;
        }

        private void list_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            choosedIndex = e.ItemIndex;
            StartButton.IsEnabled = true;
            StartButton.FadeTo(1, App.fadingTimeConst);
        }

        private void GameTypeFilterButton_Pressed(object sender, EventArgs e)
        {
            // Go to filter page
        }

        private void StartButton_Pressed(object sender, EventArgs e)
        {
            if (App.currentAccount.nickname != null)
            {
                if (choosedIndex != -1)
                    App.connectionThread = new System.Threading.Thread(unused => App.connectionThreadMethod("choose"));

                allPage.FadeTo(0, App.fadingTimeConst);
                Navigation.PushAsync(App.waitingPage, false);

                App.connectionThread.Start();
            }
        }
    }
}