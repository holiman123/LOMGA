using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LOMGAxam.Pages;

namespace LOMGAxam
{
    public partial class App : Application
    {
        public static ChooseConnectScreen chooseConnectScreen = new ChooseConnectScreen();
        public static CreateGamePage createGamePage = new CreateGamePage();
        public static FastGameSettingsPage fastGameSettingsPage = new FastGameSettingsPage();
        public App()
        {
            InitializeComponent();

            StartPage startPage = new StartPage();
            NavigationPage.SetHasNavigationBar(startPage, false);

            MainPage = new NavigationPage(startPage);
        }

        protected override void OnStart()
        {
            NavigationPage.SetHasNavigationBar(chooseConnectScreen, false);
            NavigationPage.SetHasNavigationBar(createGamePage, false);
            NavigationPage.SetHasNavigationBar(fastGameSettingsPage, false);
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
