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
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media.Imaging;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PhoneTemplate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page3 : Page
    {
        private float multiplier = 1;

        private ContentDialog rules = new ContentDialog() { Title = "Lotsa Slots", Content = "Click the Spin! button to spin the wheel.\nIf you get three matching icons in the center, it's a Jackpot!\nEach spin costs $1 and the Jackpot is $5.\nYou can add a multiplier to make the game more interesting.", PrimaryButtonText = "OK" };
        private ContentDialog jackpot = new ContentDialog() { Title = "💰Jackpot!💰", Content = "You hit the jackpot! 💵💵", PrimaryButtonText = "Woohoo!" };
        private ContentDialog notEnoughMoney = new ContentDialog() { Title = "Not Enough Money", Content = "You don't have enough money to spin that.\n\nEither lower your multiplier or top up your wallet.", PrimaryButtonText = "Go To Wallet", CloseButtonText = "Back" };

        Random randomNumGen = new Random();
        BitmapImage[] imageURLs = new BitmapImage[] { 
            new BitmapImage(new Uri("ms-appx:///Assets/Slots/MoeFace.png", UriKind.RelativeOrAbsolute)), 
            new BitmapImage(new Uri("ms-appx:///Assets/Slots/DuffCan.png", UriKind.RelativeOrAbsolute)), 
            new BitmapImage(new Uri("ms-appx:///Assets/Slots/MrSparklesFace.png", UriKind.RelativeOrAbsolute)), 
            new BitmapImage(new Uri("ms-appx:///Assets/Slots/ItchyFace.png", UriKind.RelativeOrAbsolute)) };


        public Page3()
        {
            this.InitializeComponent();

            #region Setup the device sizing for the application  
            ApplicationView.GetForCurrentView().TryResizeView(new Size(App.DeviceScreenWidth, App.DeviceScreenHeight));
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(App.DeviceMinimumScreenWidth, App.DeviceMinimumScreenHeight));
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            #endregion

            UpdateBalanceDisplay();

            ShowRules();
        }

        private async void CheckForPoor()
        {
            if (App.Balance < 1)
            {

                ContentDialogResult gotoWallet = await App.noMoney.ShowAsync();
                if (gotoWallet == ContentDialogResult.Primary)
                {
                    Frame.Navigate(typeof(Page9));
                }
            }
        }

        private async void ShowRules()
        {
            await rules.ShowAsync();
        }

        private async void MultiplierTooHighMessage()
        {
            ContentDialogResult gotoWallet = await notEnoughMoney.ShowAsync();
            if (gotoWallet == ContentDialogResult.Primary)
            {
                Frame.Navigate(typeof(Page9));
            }
        }

        private void UpdateBalanceDisplay()
        {
            TextBoxBalance.Text = "$" + App.Balance;
        }

        #region ADD YOUR CODE FOR THE PAGE HERE
        private void ButtonAddMoney_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Page10));
        }


        #endregion

        private void buttonPlay_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!float.TryParse(TextBoxMultiplier.Text, out multiplier))
            {
                TextBoxMultiplier.Text = "1";
                multiplier = 1;
            }
            if((1 * multiplier) > App.Balance && App.Balance > 0)
            {
                MultiplierTooHighMessage();
                return;
            }
            if (App.Balance > 0)
            {
                buttonPlay.IsEnabled = false;
                App.Balance -= 1 * multiplier;
                UpdateBalanceDisplay();
                SpinWheel(imageWheel1, imageWheel1Top, imageWheel1Bottom);
                SpinWheel(imageWheel2, imageWheel2Top, imageWheel2Bottom);
                SpinWheel(imageWheel3, imageWheel3Top, imageWheel3Bottom);
                buttonPlay.IsEnabled = true;

                if (CheckForJackpot(imageWheel1, imageWheel2, imageWheel3))
                {
                    App.Balance += 5 * multiplier;
                    App.slotsProfit += 5 * multiplier;
                    UpdateBalanceDisplay();
                    jackpot.ShowAsync();
                }
                return;
            }
            
            CheckForPoor();
        }

        private void SpinWheel(Image mainImage, Image topImage, Image bottomImage)
        {
            int spin = randomNumGen.Next(0, imageURLs.Length);
            mainImage.Source = imageURLs[spin];
            if(spin <= 0)
            {
                topImage.Source = imageURLs[imageURLs.Length-1];
            }
            else
            {
                topImage.Source = imageURLs[spin - 1];
            }

            if(spin >= imageURLs.Length - 1)
            {
                bottomImage.Source = imageURLs[0];
            }
            else
            {
                bottomImage.Source = imageURLs[spin + 1];
            }


        }

        private bool CheckForJackpot(Image imageLeft, Image imageMiddle, Image imageRight)
        {
            if (imageLeft.Source == imageMiddle.Source && imageLeft.Source == imageRight.Source)
                return true;
            return false;
        }
    }
}
