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
using Windows.Media.SpeechSynthesis;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PhoneTemplate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page2 : Page
    {
        SpeechSynthesizer synthesizer;

        Dictionary<int, Drink> drinks = new Dictionary<int, Drink> {
            {0, new Drink("Duff Beer", "Springfield's most popular beer.", "Beer, 4%", new BitmapImage(new Uri("ms-appx:///Assets/Drinks/DuffBeer.png", UriKind.RelativeOrAbsolute))) },
            {1, new Drink("Rum & Coke", "A classic Rum and Coke.", "Rum, 7%", new BitmapImage(new Uri("ms-appx:///Assets/Drinks/PaleAle.jpg", UriKind.RelativeOrAbsolute))) },
            {2, new Drink("Pale Ale", "Your choice from the selection of Pale Ales on tap.", "Ale, 3%", new BitmapImage(new Uri("ms-appx:///Assets/Drinks/RumCoke.png", UriKind.RelativeOrAbsolute))) },
            {3, new Drink("Old Fashioned", "Your choice of either whiskey or bourbon, sugar, Angostura bitters and an orange peel.", "Whiskey, 3% / Bourbon 3%", new BitmapImage(new Uri("ms-appx:///Assets/Drinks/OldFashioned.jpg", UriKind.RelativeOrAbsolute))) },
            {4, new Drink("Bourbon & Coke", "A classic Bourbon and Coke.", "Bourbon, 7%", new BitmapImage(new Uri("ms-appx:///Assets/Drinks/BourbonCoke.jpg", UriKind.RelativeOrAbsolute))) },
            {5, new Drink("Absolut Krusty", "Krusty the Clown's signature Vodka.", "Vodka, 7%", new BitmapImage(new Uri("ms-appx:///Assets/Drinks/AbsolutKrusty.jpg", UriKind.RelativeOrAbsolute))) }
};

        public Page2()
        {
            this.InitializeComponent();

            UpdateBalanceDisplay();

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

            DrinksListView.ItemsSource = Drink.allDrinknames;

            #region Setup the device sizing for the application  
            ApplicationView.GetForCurrentView().TryResizeView(new Size(App.DeviceScreenWidth, App.DeviceScreenHeight));
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(App.DeviceMinimumScreenWidth, App.DeviceMinimumScreenHeight));
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            #endregion
        }

        #region ADD YOUR CODE FOR THE PAGE HERE
        private void UpdateBalanceDisplay()
        {
            TextBoxBalance.Text = "$" + App.Balance;
        }

        private void ButtonAddMoney_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Page10));
        }

        #endregion

        private void DrinksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DrinksListView.SelectedValue != null)
            {
                TextBoxName.Text = drinks.ElementAt(DrinksListView.SelectedIndex).Value.Name;
                TextBlockRecipe.Text = drinks.ElementAt(DrinksListView.SelectedIndex).Value.Description;
                TextBlockMix.Text = drinks.ElementAt(DrinksListView.SelectedIndex).Value.AdditionalInfo;
                ImageDrink.Source = drinks.ElementAt(DrinksListView.SelectedIndex).Value.Image;
            }
        }

        private void ButtonReadName_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Say(TextBoxName.Text + ". " + TextBlockRecipe.Text + " " + TextBlockMix.Text);
        }

        private async void Say(string message)
        {
            SpeechSynthesisStream words = await synthesizer.SynthesizeTextToStreamAsync(message);
            media.SetSource(words, words.ContentType);
            media.Play();
        }
    }

    public class Drink
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string AdditionalInfo { get; private set; }
        public BitmapImage Image {get; private set; }

        public static List<string> allDrinknames = new List<string>();

        public Drink(string name, string desc, string addinfo, BitmapImage img)
        {
            Name = name;
            Description = desc;
            AdditionalInfo = addinfo;
            Image = img;
            Drink.allDrinknames.Add(name);
        }
    }
}
