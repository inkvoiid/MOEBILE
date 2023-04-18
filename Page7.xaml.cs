using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
// Setup the device sizing for the application
using Windows.UI.Xaml.Media.Imaging;

// Setup the device sizing for the application
using Windows.UI.ViewManagement;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Popups;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PhoneTemplate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page7 : Page
    {
        private ContentDialog rulesMessage = new ContentDialog() { 
            Title = "Dicey Sixes", 
            Content = "This is a dice rolling game where you have to roll five sixes.\nTo stop a die from being rolled, simply tap on the die you want to hold.\nOne game costs $5 and if you win, you get $10, but you can change the multiplier for a more interesting game.", 
            PrimaryButtonText = "OK" };


        private ContentDialog youLose = new ContentDialog() { 
            Title = "You Lose!", 
            Content = "Play again?", 
            CloseButtonText = "No...", 
            PrimaryButtonText = "You bet" };

        private ContentDialog youWin = new ContentDialog()
        {
            Title = "You Win, I guess.",
            Content = "You must have cheated! Let's go again.",
            CloseButtonText = "No...",
            PrimaryButtonText = "Alright!"
        };

        DispatcherTimer timeBetweenPlayers; // Timer to make computer look like they're thinking.

        private BitmapImage[] diceImages = new BitmapImage[] {
        new BitmapImage(new Uri("ms-appx:///Assets/DiceGame/DiceSides/one.png", UriKind.RelativeOrAbsolute)),
        new BitmapImage(new Uri("ms-appx:///Assets/DiceGame/DiceSides/two.png", UriKind.RelativeOrAbsolute)),
        new BitmapImage(new Uri("ms-appx:///Assets/DiceGame/DiceSides/three.png", UriKind.RelativeOrAbsolute)),
        new BitmapImage(new Uri("ms-appx:///Assets/DiceGame/DiceSides/four.png", UriKind.RelativeOrAbsolute))
        ,new BitmapImage(new Uri("ms-appx:///Assets/DiceGame/DiceSides/five.png", UriKind.RelativeOrAbsolute))
        ,new BitmapImage(new Uri("ms-appx:///Assets/DiceGame/DiceSides/six.png", UriKind.RelativeOrAbsolute))};
        int timerLength = 1; // Timer length in seconds

        #region Random Number setup
        Random number;
        #endregion

        public Page7()
        {
            this.InitializeComponent();

            timeBetweenPlayers = new DispatcherTimer();
            timeBetweenPlayers.Tick += TimeBetweenPlayers_Tick;
            timeBetweenPlayers.Interval = new TimeSpan(0, 0, timerLength);

            UpdateBalanceDisplay();

            selectMoe.Visibility = Visibility.Collapsed;
            selectPlayer.Visibility = Visibility.Visible;

            TextBoxMultiplier.Text = "" + App.diceGameMultiplier;
            CheckForOngoingGame();

            #region Random Number setup
            number = new Random(DateTime.Now.Millisecond);
            #endregion

            #region Setup the device sizing for the application  
            ApplicationView.GetForCurrentView().TryResizeView(new Size(App.DeviceScreenWidth, App.DeviceScreenHeight));
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(App.DeviceMinimumScreenWidth, App.DeviceMinimumScreenHeight));
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            #endregion
        }

        private void CheckForOngoingGame()
        {
            if(App.diceGameIsPlaying == true)
            {
                TextBoxMultiplier.IsEnabled = false;
                #region Set Dice To Stored States
                // Moe's Dice
                imageDice1.Opacity = App.diceGameMoeDiceHeld[0] == true ? 0.5f : 1f;

                imageDice2.Opacity = App.diceGameMoeDiceHeld[1] == true ? 0.5f : 1f;

                imageDice3.Opacity = App.diceGameMoeDiceHeld[2] == true ? 0.5f : 1f;

                imageDice4.Opacity = App.diceGameMoeDiceHeld[3] == true ? 0.5f : 1f;

                imageDice6.Opacity = App.diceGameMoeDiceHeld[4] == true ? 0.5f : 1f;

                // Player's Dice
                imageDice6.Opacity = App.diceGamePlayerDiceHeld[0] == true ? 0.5f : 1f;

                imageDice7.Opacity = App.diceGamePlayerDiceHeld[1] == true ? 0.5f : 1f;

                imageDice8.Opacity = App.diceGamePlayerDiceHeld[2] == true ? 0.5f : 1f;

                imageDice9.Opacity = App.diceGamePlayerDiceHeld[3] == true ? 0.5f : 1f;

                imageDice10.Opacity = App.diceGamePlayerDiceHeld[4] == true ? 0.5f : 1f;
                #endregion

                #region Set Dice To Correct Values
                SetMoeDiceImages();
                SetPlayerDiceImages();
                #endregion
            }
            else
            {
            ShowRules();
            }
                
        }

        private void ButtonAddMoney_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Page10));
        }

        private async void ShowRules()
        {
            await rulesMessage.ShowAsync();
        }
        private async void StartGame()
        {
            ContentDialog startGameMessage = new ContentDialog()
            {
                Title = "Start a game?",
                Content = "Start a game with a " + App.diceGameMultiplier + "x multiplier ($" + (5 * App.diceGameMultiplier) + ")",
                CloseButtonText = "No",
                PrimaryButtonText = "Yes"
            };
        ContentDialogResult wantToPlay = await startGameMessage.ShowAsync();
            if (wantToPlay == ContentDialogResult.Primary)
            {
                SetupGame();
            }
            else 
            { 
                ResetGame(); 
            }
        }

        private void SetupGame()
        {
            CheckForPoor();
            if (App.Balance >= (5 * App.diceGameMultiplier))
            {
                App.Balance -= 5 * App.diceGameMultiplier;
                UpdateBalanceDisplay();
                App.diceGameMoeDiceNumbers = new int[5] { 1, 1, 1, 1, 1 };
                App.diceGamePlayerDiceNumbers = new int[5] { 1, 1, 1, 1, 1 };
                App.diceGameMoeDiceHeld = new bool[5] { false, false, false, false, false };
                App.diceGamePlayerDiceHeld = new bool[5] { false, false, false, false, false };
                imageDice1.Opacity = 1f;
                imageDice2.Opacity = 1f;
                imageDice3.Opacity = 1f;
                imageDice4.Opacity = 1f;
                imageDice5.Opacity = 1f;
                imageDice6.Opacity = 1f;
                imageDice7.Opacity = 1f;
                imageDice8.Opacity = 1f;
                imageDice9.Opacity = 1f;
                imageDice10.Opacity = 1f;
                ButtonDiceRoll.IsEnabled = true;
                App.diceGameIsPlaying = true;
                SetMoeDiceImages();
                SetPlayerDiceImages();
            }
        }

        private void ResetGame()
        {
            UpdateBalanceDisplay();
            App.diceGameMoeDiceNumbers = new int[5] { 1, 1, 1, 1, 1 };
            App.diceGamePlayerDiceNumbers = new int[5] { 1, 1, 1, 1, 1 };
            App.diceGameMoeDiceHeld = new bool[5] { false, false, false, false, false };
            App.diceGamePlayerDiceHeld = new bool[5] { false, false, false, false, false };
            imageDice1.Opacity = 1f;
            imageDice2.Opacity = 1f;
            imageDice3.Opacity = 1f;
            imageDice4.Opacity = 1f;
            imageDice5.Opacity = 1f;
            imageDice6.Opacity = 1f;
            imageDice7.Opacity = 1f;
            imageDice8.Opacity = 1f;
            imageDice9.Opacity = 1f;
            imageDice10.Opacity = 1f;
            ButtonDiceRoll.IsEnabled = true;
            App.diceGameIsPlaying = false;
            TextBoxMultiplier.IsEnabled = true;
            SetMoeDiceImages();
            SetPlayerDiceImages();
        }

        private async void CheckForPoor()
        {
            UpdateBalanceDisplay();
            if (App.Balance < 1)
            {

                ContentDialogResult gotoWallet = await App.noMoney.ShowAsync();
                if (gotoWallet == ContentDialogResult.Primary)
                {
                    Frame.Navigate(typeof(Page9));
                }
                else
                {
                    Frame.Navigate(typeof(Page1));
                }
            }

            else if (App.Balance < (5 * App.diceGameMultiplier))
            {
                ContentDialog notEnoughMoney = new ContentDialog()
                {
                    Title = "Not Enough Money",
                    Content = "This game with a " + App.diceGameMultiplier + "x multiplier costs $" + (5 * App.diceGameMultiplier) + " to play.\nYou don't have enough money to play that game.\n\nEither lower the multiplier or top up your wallet.",
                    CloseButtonText = "Back",
                    PrimaryButtonText = "Go to Wallet"
                };
                ContentDialogResult gotoWallet = await notEnoughMoney.ShowAsync();
                if (gotoWallet == ContentDialogResult.Primary)
                {
                    Frame.Navigate(typeof(Page9));
                }
                else
                {
                    Frame.Navigate(typeof(Page1));
                }
            }
        }

        private void UpdateBalanceDisplay()
        {
            TextBoxBalance.Text = "$" + App.Balance;
        }

        private void TimeBetweenPlayers_Tick(object sender, object e)
        {
            MoeRoll();
        }

        #region Player 1

        private void MoeRoll()
        {
            for (int i = 0; i < App.diceGameMoeDiceNumbers.Length; i++)
            {
                if (!App.diceGameMoeDiceHeld[i])
                {
                    App.diceGameMoeDiceNumbers[i] = number.Next(1, 7);
                }


                SetMoeDiceImages();
            }

            for (int i = 0; i < App.diceGameMoeDiceNumbers.Length; i++)
            {
                if (App.diceGameMoeDiceNumbers[i] != 6)
                    break;
                if (i < App.diceGameMoeDiceNumbers.Length - 1)
                    continue;
                else
                    MoeVictory();
            }

            timeBetweenPlayers.Stop();
            ButtonDiceRoll.IsEnabled = true;
            selectMoe.Visibility = Visibility.Collapsed;
            selectPlayer.Visibility = Visibility.Visible;
        }

        private void imageDice1_Tapped()
        {
            if (App.diceGameMoeDiceNumbers[0] == 6)
            {
                App.diceGameMoeDiceHeld[0] = true;
                imageDice1.Opacity = App.diceGameMoeDiceHeld[0] == true ? 0.5f : 1f;
            }
        }
        private void imageDice2_Tapped()
        {
            if (App.diceGameMoeDiceNumbers[1] == 6)
            {
                App.diceGameMoeDiceHeld[1] = true;
                imageDice2.Opacity = App.diceGameMoeDiceHeld[1] == true ? 0.5f : 1f;
            }
        }
        private void imageDice3_Tapped()
        {
            if (App.diceGameMoeDiceNumbers[2] == 6)
            {
                App.diceGameMoeDiceHeld[2] = true;
                imageDice3.Opacity = App.diceGameMoeDiceHeld[2] == true ? 0.5f : 1f;
            }
        }
        private void imageDice4_Tapped()
        {
            if (App.diceGameMoeDiceNumbers[3] == 6)
            {
                App.diceGameMoeDiceHeld[3] = true;
                imageDice4.Opacity = App.diceGameMoeDiceHeld[3] == true ? 0.5f : 1f;
            }
        }
        private void imageDice5_Tapped()
        {
            if (App.diceGameMoeDiceNumbers[4] == 6)
            {
                App.diceGameMoeDiceHeld[4] = true;
                imageDice5.Opacity = App.diceGameMoeDiceHeld[4] == true ? 0.5f : 1f;
            }
        }

        #endregion

        #region Player 2
        private void player2Roll_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(TextBoxMultiplier.Text, out App.diceGameMultiplier))
            {
                TextBoxMultiplier.Text = "1";
                App.diceGameMultiplier = 1;
            }
            if (App.diceGameIsPlaying == false)
            {
                StartGame();
            }
            else
            {
                TextBoxMultiplier.IsEnabled = false;
                App.diceGameIsPlaying = true;
                ButtonDiceRoll.IsEnabled = false;

                for (int i = 0; i < App.diceGamePlayerDiceNumbers.Length; i++)
                {
                    if (!App.diceGamePlayerDiceHeld[i])
                    {
                        App.diceGamePlayerDiceNumbers[i] = number.Next(1, 7);
                    }


                    SetPlayerDiceImages();
                }



                bool wonGame = false;
                for (int i = 0; i < App.diceGamePlayerDiceNumbers.Length; i++)
                {
                    if (App.diceGamePlayerDiceNumbers[i] != 6)
                        break;
                    if (i < App.diceGamePlayerDiceNumbers.Length - 1)
                        continue;
                    else
                        wonGame = true;
                }

                if (wonGame)
                {
                    PlayerVictory();
                }
                else
                {
                    selectMoe.Visibility = Visibility.Visible;
                    selectPlayer.Visibility = Visibility.Collapsed;
                    timeBetweenPlayers.Start();
                }
            }
        }
        private void imageDice6_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.diceGamePlayerDiceNumbers[0] == 6)
            {
                App.diceGamePlayerDiceHeld[0] = !App.diceGamePlayerDiceHeld[0];
                imageDice6.Opacity = App.diceGamePlayerDiceHeld[0] == true ? 0.5f : 1f;
            }
        }
        private void imageDice7_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.diceGamePlayerDiceNumbers[1] == 6)
            {
                App.diceGamePlayerDiceHeld[1] = !App.diceGamePlayerDiceHeld[1];
                imageDice7.Opacity = App.diceGamePlayerDiceHeld[1] == true ? 0.5f : 1f;
            }
        }
        private void imageDice8_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.diceGamePlayerDiceNumbers[2] == 6)
            {
                App.diceGamePlayerDiceHeld[2] = !App.diceGamePlayerDiceHeld[2];
                imageDice8.Opacity = App.diceGamePlayerDiceHeld[2] == true ? 0.5f : 1f;
            }
        }
        private void imageDice9_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.diceGamePlayerDiceNumbers[3] == 6)
            {
                App.diceGamePlayerDiceHeld[3] = !App.diceGamePlayerDiceHeld[3];
                imageDice9.Opacity = App.diceGamePlayerDiceHeld[3] == true ? 0.5f : 1f;
            }
        }
        private void imageDice10_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.diceGamePlayerDiceNumbers[4] == 6)
            {
                App.diceGamePlayerDiceHeld[4] = !App.diceGamePlayerDiceHeld[4];
                imageDice10.Opacity = App.diceGamePlayerDiceHeld[4] == true ? 0.5f : 1f;
            }
        }
        #endregion

        private void SetMoeDiceImages()
        {
            for (int i = 0; i < App.diceGameMoeDiceNumbers.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        imageDice1.Source = diceImages[App.diceGameMoeDiceNumbers[i] - 1];
                        if (App.diceGameMoeDiceNumbers[i] == 6)
                            imageDice1_Tapped();
                        break;
                    case 1:
                        imageDice2.Source = diceImages[App.diceGameMoeDiceNumbers[i] - 1];
                        if (App.diceGameMoeDiceNumbers[i] == 6)
                            imageDice2_Tapped();
                        break;
                    case 2:
                        imageDice3.Source = diceImages[App.diceGameMoeDiceNumbers[i] - 1];
                        if (App.diceGameMoeDiceNumbers[i] == 6)
                            imageDice3_Tapped();
                        break;
                    case 3:
                        imageDice4.Source = diceImages[App.diceGameMoeDiceNumbers[i] - 1];
                        if (App.diceGameMoeDiceNumbers[i] == 6)
                            imageDice4_Tapped();
                        break;
                    case 4:
                        imageDice5.Source = diceImages[App.diceGameMoeDiceNumbers[i] - 1];
                        if (App.diceGameMoeDiceNumbers[i] == 6)
                            imageDice5_Tapped();
                        break;
                }
            }
        }

        private void SetPlayerDiceImages(){
            for (int i = 0; i < App.diceGamePlayerDiceNumbers.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        imageDice6.Source = diceImages[App.diceGamePlayerDiceNumbers[i] - 1];
                        break;
                    case 1:
                        imageDice7.Source = diceImages[App.diceGamePlayerDiceNumbers[i] - 1];
                        break;
                    case 2:
                        imageDice8.Source = diceImages[App.diceGamePlayerDiceNumbers[i] - 1];
                        break;
                    case 3:
                        imageDice9.Source = diceImages[App.diceGamePlayerDiceNumbers[i] - 1];
                        break;
                    case 4:
                        imageDice10.Source = diceImages[App.diceGamePlayerDiceNumbers[i] - 1];
                        break;
                }
            }
        }

        private async void MoeVictory()
        {
            App.diceGameIsPlaying = false;
            ContentDialogResult playAgain = await youLose.ShowAsync();
            if (playAgain == ContentDialogResult.Primary)
            {
                StartGame();
            }
            else
            {
                Frame.Navigate(typeof(Page1));
            }
        }

        private async void PlayerVictory()
        {
            App.diceGameIsPlaying = false;
            App.Balance += 10 * App.diceGameMultiplier;
            App.diceGameProfit += 10 * App.diceGameMultiplier;
            UpdateBalanceDisplay();
            ContentDialogResult playAgain = await youWin.ShowAsync();
            if (playAgain == ContentDialogResult.Primary)
            {
                StartGame();
            }
            else
            {
                Frame.Navigate(typeof(Page1));
            }
        }
    }
}
