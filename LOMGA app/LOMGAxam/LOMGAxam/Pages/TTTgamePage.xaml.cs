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
        int[,] localField;

        int turnX = -1;
        int turnY = -1;

        public TTTgamePage()
        {
            InitializeComponent();

            createfield();
        }

        private void Turn_Button_Pressed(object sender, EventArgs e)
        {
            // turn method calling
            if (turnX != -1 && turnY != -1)
            {
                Field.IsEnabled = false;
                TurnButton.IsEnabled = false;

                label.Text = "Opponents turn";
                ((GameClassTTT)App.currentGame).turn(turnX, turnY);

                byte[] data = MySerializer.serialize(App.currentGame);
                App.stream.Write(data, 0, data.Length);

                turnX = -1;
                turnY = -1;
            }
        }

        public void recive()
        {
            label.Text = "Your turn";
            Field.IsEnabled = true;

            for (int i = 0; i < ((GameClassTTT)App.currentGame).rowSize; i++)
                for (int j = 0; j < ((GameClassTTT)App.currentGame).columnSize; j++)
                    localField[i, j] = ((GameClassTTT)App.currentGame).field[j, i];

            drawLocalField();
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
            int pressedX = Convert.ToInt32(((Button)sender).AutomationId.Split(' ')[0]);
            int pressedY = Convert.ToInt32(((Button)sender).AutomationId.Split(' ')[1]);

            if (localField[pressedX, pressedY] == 0)
            {
                for(int i = 0; i < ((GameClassTTT)App.currentGame).rowSize; i++)
                    for (int j = 0; j < ((GameClassTTT)App.currentGame).columnSize; j++)
                        localField[i, j] = ((GameClassTTT)App.currentGame).field[j, i];

                if (((GameClassTTT)App.currentGame).turnFlag)
                    localField[pressedX, pressedY] = 1;
                else
                    localField[pressedX, pressedY] = 2;

                TurnButton.IsEnabled = true;

                turnX = pressedX;
                turnY = pressedY;

                drawLocalField();
            }
        }

        void setCellState(int x, int y, int state)
        {
            List<View> views = Field.Children.ToList();
            List<Image> images = new List<Image>();

            foreach (View view in views)
            {
                images.Add((Image)((AbsoluteLayout)view).Children.ToList()[1]);
                images.Add((Image)((AbsoluteLayout)view).Children.ToList()[2]);
            }

            if (state == 0)
            {
                foreach (Image img in images)
                {
                    if (img.AutomationId.Split(' ')[1] == x + "" && img.AutomationId.Split(' ')[2] == y + "")
                        img.FadeTo(0, 150);
                }
            }
            if (state == 1)
            {
                foreach (Image img in images)
                {
                    if (img.AutomationId.Split(' ')[0] == "zero" && img.AutomationId.Split(' ')[1] == x + "" && img.AutomationId.Split(' ')[2] == y + "")
                        img.FadeTo(0, 150);
                    if (img.AutomationId.Split(' ')[0] == "cross" && img.AutomationId.Split(' ')[1] == x + "" && img.AutomationId.Split(' ')[2] == y + "")
                        img.FadeTo(1, 150);
                }
            }
            if (state == 2)
            {
                foreach (Image img in images)
                {
                    if (img.AutomationId.Split(' ')[0] == "cross" && img.AutomationId.Split(' ')[1] == x + "" && img.AutomationId.Split(' ')[2] == y + "")
                        img.FadeTo(0, 150);
                    if (img.AutomationId.Split(' ')[0] == "zero" && img.AutomationId.Split(' ')[1] == x + "" && img.AutomationId.Split(' ')[2] == y + "")
                        img.FadeTo(1, 150);
                }
            }
        }

        void drawLocalField()
        {
            for (int i = 0; i < ((GameClassTTT)App.currentGame).rowSize; i++)
                for (int j = 0; j < ((GameClassTTT)App.currentGame).columnSize; j++)
                {
                    setCellState(i, j, localField[i, j]);
                }
        }

        void createfield()
        {
            // TEMP commented
            int rowCount = ((GameClassTTT)App.currentGame).rowSize;
            int columnCount = ((GameClassTTT)App.currentGame).columnSize;

            localField = new int[rowCount,columnCount];

            //int rowCount = 3;
            //int columnCount = 3;

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
                    
                    Image cross = new Image { Source = "cross.png", Opacity = 0, AutomationId = "cross " + i + " " + j };
                    Image zero = new Image { Source = "zero.png", Opacity = 0, AutomationId = "zero " + i + " " + j };
                    Frame backgroundFrame = new Frame { BackgroundColor = Color.FromHex("C4C4C4"), CornerRadius = 5, HasShadow = false };
                    Button button = new Button { Opacity = 0, AutomationId = i + " " + j };
                    button.Clicked += field_turn_Pressed;

                    AbsoluteLayout.SetLayoutBounds(button, new Rectangle(0.5, 0.5, 1, 1));
                    AbsoluteLayout.SetLayoutFlags(button, AbsoluteLayoutFlags.All);

                    AbsoluteLayout.SetLayoutBounds(cross, new Rectangle(0.5, 0.5, 0.8, 0.8));
                    AbsoluteLayout.SetLayoutFlags(cross, AbsoluteLayoutFlags.All);

                    AbsoluteLayout.SetLayoutBounds(zero, new Rectangle(0.5, 0.5, 0.8, 0.8));
                    AbsoluteLayout.SetLayoutFlags(zero, AbsoluteLayoutFlags.All);

                    AbsoluteLayout.SetLayoutBounds(backgroundFrame, new Rectangle(0.5, 0.5, 0.95, 0.95));
                    AbsoluteLayout.SetLayoutFlags(backgroundFrame, AbsoluteLayoutFlags.All);

                    tempLayout.Children.Add(backgroundFrame);
                    tempLayout.Children.Add(cross);
                    tempLayout.Children.Add(zero);
                    tempLayout.Children.Add(button);

                    Field.Children.Add(tempLayout, i, j);
                }
        }
    }
}