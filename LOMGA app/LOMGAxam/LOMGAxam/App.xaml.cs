using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LOMGAxam.Pages;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text;
using LOMGAgameClass;

namespace LOMGAxam
{
    public partial class App : Application
    {
        public const int fadingTimeConst = 200;
        public static Thread connectionThread;

        public static ChooseConnectScreen chooseConnectScreen = new ChooseConnectScreen();
        public static CreateGamePage createGamePage = new CreateGamePage();
        public static FastGameSettingsPage fastGameSettingsPage = new FastGameSettingsPage();
        public static newTTTgameSettingsPage newTTTgameSettingsPage = new newTTTgameSettingsPage();
        public static WaitingPage waitingPage = new WaitingPage();
        public static TTTgamePage tttGamePage = new TTTgamePage();
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
            NavigationPage.SetHasNavigationBar(newTTTgameSettingsPage, false);
            NavigationPage.SetHasNavigationBar(waitingPage, false);
            NavigationPage.SetHasNavigationBar(tttGamePage, false);
        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }

        public static void connectionThreadMethod(string modeStr, INavigation page)
        {
            if (modeStr.Split(',')[0] == "start")
            {
                TcpClient client = new TcpClient("127.0.0.1", 2003);
                NetworkStream stream = client.GetStream();

                byte[] data = Encoding.Default.GetBytes(modeStr);
                stream.Write(data, 0, data.Length);

                data = new byte[1024];
                stream.Read(data, 0, 1024);
                Game recivedgame = (Game)MySerializer.deserialize(data);

                page.PushAsync(tttGamePage, false);

            }
        }
    }
}
