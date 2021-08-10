using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOMGAgameClass;
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

            createfield();
        }

        private void Turn_Button_Pressed(object sender, EventArgs e)
        {
            // TODO: turn method calling
            byte[] data = MySerializer.serialize(App.currentGame);
            App.stream.Write(data, 0, data.Length);
        }

        protected override void OnAppearing()
        {
            allPage.FadeTo(1, App.fadingTimeConst);
            base.OnAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
            //-----------------------------------
            App.connectionThread.Abort();           //      ---TODO---
            Navigation.PopToRootAsync(false);       //  This section MUST be in 'Yes' button for leaving 'Pressed'
            //-----------------------------------
            return true;
        }

        private void field_turn_Pressed(object sender, EventArgs e)
        {
            // TODO: turn calc!
        }

        void createfield()
        {
            // TEMP commented
            //int rowCount = ((GameClassTTT)App.currentGame).rowSize;
            //int columnCount = ((GameClassTTT)App.currentGame).columnSize;

            int rowCount = 3;       // TEMP
            int columnCount = 3;    // TEMP

            for (int i = 0; i < rowCount; i++)
            {
                Field.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
            for (int i = 0; i < columnCount; i++)
            {
                Field.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            for (int i = 0; i < columnCount; i++)
                for (int j = 0; j < rowCount; j++)
                {
                    AbsoluteLayout tempLayout = new AbsoluteLayout();
                    
                    Image cross = new Image { Source = "cross.png", Opacity = 0 };
                    Image zero = new Image { Source = "zero.png", Opacity = 0 };
                    Frame backgroundFrame = new Frame { BackgroundColor = Color.FromHex("C4C4C4"), CornerRadius = 5, HasShadow = false };
                    Button button = new Button { Opacity = 0, AutomationId = i + " " + j };
                    button.Clicked += field_turn_Pressed;

                    AbsoluteLayout.SetLayoutBounds(cross, new Rectangle(0.5, 0.5, 0.8, 0.8));
                    AbsoluteLayout.SetLayoutBounds(zero, new Rectangle(0.5, 0.5, 0.8, 0.8));
                    AbsoluteLayout.SetLayoutFlags(cross, AbsoluteLayoutFlags.All);
                    AbsoluteLayout.SetLayoutFlags(zero, AbsoluteLayoutFlags.All);
                    AbsoluteLayout.SetLayoutBounds(backgroundFrame, new Rectangle(0.5, 0.5, 0.95, 0.95));
                    AbsoluteLayout.SetLayoutFlags(backgroundFrame, AbsoluteLayoutFlags.All);

                    tempLayout.Children.Add(backgroundFrame);
                    tempLayout.Children.Add(cross);
                    tempLayout.Children.Add(zero);

                    Field.Children.Add(tempLayout, i, j);
                }
        }
    }
}