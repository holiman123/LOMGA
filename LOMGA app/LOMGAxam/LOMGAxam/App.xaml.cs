using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LOMGAxam.Pages;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text;
using LOMGAgameClass;
using Xamarin.Essentials;

namespace LOMGAxam
{
    public partial class App : Application
    {
        public const int fadingTimeConst = 200;
        public static Thread connectionThread;
        public static INavigation NavigationStatic;

        public static NetworkStream stream;
        public static Game currentGame;

        public static ChooseConnectScreen chooseConnectScreen = new ChooseConnectScreen();
        public static CreateGamePage createGamePage = new CreateGamePage();
        public static FastGameSettingsPage fastGameSettingsPage = new FastGameSettingsPage();
        public static newTTTgameSettingsPage newTTTgameSettingsPage = new newTTTgameSettingsPage();
        public static WaitingPage waitingPage = new WaitingPage();
        public static TTTgamePage tttGamePage;
        public App()
        {
            InitializeComponent();

            StartPage startPage = new StartPage();
            NavigationPage.SetHasNavigationBar(startPage, false);

            MainPage = new NavigationPage(startPage); // TEMP! startPage is current page to start!
            NavigationStatic = MainPage.Navigation;
        }

        protected override void OnStart()
        {
            NavigationPage.SetHasNavigationBar(chooseConnectScreen, false);
            NavigationPage.SetHasNavigationBar(createGamePage, false);
            NavigationPage.SetHasNavigationBar(fastGameSettingsPage, false);
            NavigationPage.SetHasNavigationBar(newTTTgameSettingsPage, false);
            NavigationPage.SetHasNavigationBar(waitingPage, false);
        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }

        /// <summary>
        /// Thread to operate connection to server
        /// (start - start new game / list get list of games and connect ot them)
        /// </summary>
        /// <param name="modeStr">String that shows mode to start thread</param>
        public static void connectionThreadMethod(string modeStr)
        {
            if (modeStr.Split(',')[0] == "start")
            {
                TcpClient client = new TcpClient("192.168.0.102", 2003);
                stream = client.GetStream();

                byte[] data = Encoding.Default.GetBytes(modeStr);
                stream.Write(data, 0, data.Length);

                data = new byte[1024];
                stream.Read(data, 0, 1024);
                currentGame = (Game)MySerializer.deserialize(data);

                App.tttGamePage = new TTTgamePage();
                NavigationPage.SetHasNavigationBar(App.tttGamePage, false);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    NavigationStatic.PushAsync(tttGamePage, false); // TODO : "switch" statement for other games
                });

                while (true)
                {
                    data = new byte[1024];
                    stream.Read(data, 0, 1024);
                    currentGame = (Game)MySerializer.deserialize(data);

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        tttGamePage.recive(); // TODO: "switch" statement for other games   
                    });
                }
            }
        }
    }
}
