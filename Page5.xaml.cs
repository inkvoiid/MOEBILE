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
using Windows.Media.SpeechSynthesis;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PhoneTemplate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page5 : Page
    {
        Random rand;
        SpeechSynthesizer synthesizer;

        string[] time;
        string[] aspect;
        string[] effect;
        string[] persona;
        string[] feature;
        string[] consequence;
        public Page5()
        {
            this.InitializeComponent();

            #region Setup the device sizing for the application  
            ApplicationView.GetForCurrentView().TryResizeView(new Size(App.DeviceScreenWidth, App.DeviceScreenHeight));
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(App.DeviceMinimumScreenWidth, App.DeviceMinimumScreenHeight));
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            #endregion

            rand = new Random();
            time = new string[] { "a week", rand.Next(0, 11) + " weeks", "a fortnight", "a month", "a year" };
            aspect = new string[] { "bank account(s)", "conversations", "alcoholism"};
            effect = new string[] { "become fun", "become frustrating", "become wacky", "become zany", "become empty", "halt"};
            persona = new string[] { "bartender", "tavernkeep", "single, middle aged man", "small business owner"};
            feature = new string[] { "curly grey hair", "a blue apron", "a blue bowtie", "a powder blue shirt"};
            consequence = new string[] { "give five bucks to", "give ten bucks to", "go on a date with", "pay his taxes", "compliment"};


            try
            {
                // Configure the audio output.
                synthesizer = new SpeechSynthesizer();
            }
            catch (System.IO.FileNotFoundException)
            {
                // Voice Package not installed
                var messageDialog = new Windows.UI.Popups.MessageDialog("Media player components unavailable.\nYou need to Install a Voice Package in your Windows Settings.\n\nSettings > Time & Language > Speech > Manage Voices > Add Voices");
                messageDialog.ShowAsync();
            }
            catch (Exception)
            {
                // If the text is unable to be synthesized, throw an error message to the user.
                media.AutoPlay = false;
                var messageDialog = new Windows.UI.Popups.MessageDialog("Unable to synthesize text");
                messageDialog.ShowAsync();
            }

            TextBlockPrediction.Text = "Hello dear, for just $5 I can read you your fortune!";
            UpdateBalanceDisplay();
        }

        private void UpdateBalanceDisplay()
        {
            TextBoxBalance.Text = "$" + App.Balance;
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
            else if(App.Balance < 5)
            {
                ContentDialog notEnoughMoney = new ContentDialog() { Title = "Not Enough Money", Content = "You don't have enough money to have your fortune read\n\nWould you like to top up your wallet.", PrimaryButtonText = "Sure!", CloseButtonText = "No..." };
                ContentDialogResult gotoWallet = await notEnoughMoney.ShowAsync();
                if (gotoWallet == ContentDialogResult.Primary)
                {
                    Frame.Navigate(typeof(Page9));
                }
            }
        }

        #region ADD YOUR CODE FOR THE PAGE HERE
        private void ButtonAddMoney_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Page10));
        }


        #endregion

        private void ButtonPrediction_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(App.Balance >= 5)
            {
                App.Balance -= 5;
                UpdateBalanceDisplay();
                TextBlockPrediction.Text = "Over a period of " + time[rand.Next(0,time.Length)] + " your "+aspect[rand.Next(0,aspect.Length)] +" will "+effect[rand.Next(0,effect.Length)]+". This will come to pass after you meet a "+persona[rand.Next(0,persona.Length)]+" with "+feature[rand.Next(0,feature.Length)] +" who for some reason you find yourself obliged to "+consequence[rand.Next(0,consequence.Length)]+".";
                return;
            }
            CheckForPoor();
        }

        private async void Say(string message)
        {
            SpeechSynthesisStream words = await synthesizer.SynthesizeTextToStreamAsync(message);
            media.SetSource(words, words.ContentType);
            media.Play();
        }

        private void ButtonReadPrediction_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Say(TextBlockPrediction.Text);
        }
    }
}
