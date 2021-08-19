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

        public static TcpClient client = new TcpClient();
        public static NetworkStream stream;
        public static Game currentGame;
        public static Account currentAccount;

        public static ChooseConnectScreen chooseConnectScreen = new ChooseConnectScreen();
        public static CreateGamePage createGamePage = new CreateGamePage();
        public static FastGameSettingsPage fastGameSettingsPage = new FastGameSettingsPage();
        public static newTTTgameSettingsPage newTTTgameSettingsPage = new newTTTgameSettingsPage();
        public static WaitingPage waitingPage = new WaitingPage();
        public static TTTgamePage tttGamePage;
        public static ListPage listPage = new ListPage();
        public App()
        {
            InitializeComponent();

            StartPage startPage = new StartPage();
            NavigationPage.SetHasNavigationBar(startPage, false);

            MainPage = new NavigationPage(startPage);
            NavigationStatic = MainPage.Navigation;
            currentAccount = new Account();
        }

        protected override void OnStart()
        {
            NavigationPage.SetHasNavigationBar(chooseConnectScreen, false);
            NavigationPage.SetHasNavigationBar(createGamePage, false);
            NavigationPage.SetHasNavigationBar(fastGameSettingsPage, false);
            NavigationPage.SetHasNavigationBar(newTTTgameSettingsPage, false);
            NavigationPage.SetHasNavigationBar(waitingPage, false);
            NavigationPage.SetHasNavigationBar(listPage, false);
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
                try
                {
                    client = new TcpClient();
                    client.Connect(IPAddress.Parse("192.168.0.102"), 2003);
                    stream = client.GetStream();

                    byte[] data = Encoding.Default.GetBytes(modeStr);
                    stream.Write(data, 0, data.Length);

                    data = MySerializer.serialize(currentGame);
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

                    while (client.Connected)
                    {
                        data = new byte[1024];
                        stream.Read(data, 0, 1024);
                        currentGame = (Game)MySerializer.deserialize(data);

                        if (!(currentGame is null))
                        {
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                tttGamePage.recive(); // TODO: "switch" statement for other games
                            });
                        }
                        else
                            break;
                    }

                    stream.Close();
                    client.Close();

                    showErrorMessage("Lost connection with opponent");

                }
                catch (Exception e)
                {
                    stream.Close();
                    client.Close();
                    showErrorMessage("Problem with connection"); 
                }
            }

            if (modeStr.Split(',')[0] == "list")
            {
                try
                {
                    if (client == null || !client.Connected)
                    {
                        client = new TcpClient();
                        client.Connect(IPAddress.Parse("192.168.0.102"), 2003);
                        stream = client.GetStream();
                    }
                    else { }

                    byte[] data = Encoding.Default.GetBytes(modeStr);
                    stream.Write(data, 0, data.Length);

                    while (true)
                    {
                        data = new byte[1024];
                        stream.Read(data, 0, 1024);

                        if (Encoding.Default.GetString(data).Remove(4) == "end.")
                            break;

                        ListPage.recivedGames.Add((Game)MySerializer.deserialize(data));
                        stream.Write(new byte[] { 0 }, 0, 1);
                    }
                }
                catch (Exception) { }
            }

            if (modeStr.Split(',')[0] == "choose")
            {
                if (ListPage.choosedIndex != -1)
                {
                    byte[] data = Encoding.Default.GetBytes("choose,");
                    stream.Write(data, 0, data.Length);

                    ListPage.recivedGames[ListPage.choosedIndex].accounts.Add(currentAccount);

                    data = MySerializer.serialize(ListPage.recivedGames[ListPage.choosedIndex]);
                    stream.Write(data, 0, data.Length);

                    data = new byte[1024];
                    stream.Read(data, 0, data.Length);
                    currentGame = (Game)MySerializer.deserialize(data);

                    App.tttGamePage = new TTTgamePage();
                    NavigationPage.SetHasNavigationBar(App.tttGamePage, false);

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        NavigationStatic.PushAsync(tttGamePage, false); // TODO : "switch" statement for other games
                    });

                    while (client.Connected)
                    {
                        data = new byte[1024];
                        stream.Read(data, 0, 1024);
                        currentGame = (Game)MySerializer.deserialize(data);

                        if (!(currentGame is null))
                        {
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                tttGamePage.recive(); // TODO: "switch" statement for other games
                            });
                        }
                        else
                            break;
                    }

                    stream.Close();
                    client.Close();

                    showErrorMessage("Lost connection with opponent");
                }
            }
        }

        public static void showErrorMessage(string errorString)
        {
            MessagePage messagePage = new MessagePage(errorString);
            NavigationPage.SetHasNavigationBar(messagePage, false);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                NavigationStatic.PushAsync(messagePage, false);
            });
        }
    }
}
