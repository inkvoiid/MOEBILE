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
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PhoneTemplate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page6 : Page
    {
        string[] siteNames = new string[] {
            "1740 E 17th St , Cleveland, Ohio",
            "Universal Studios, Florida",
            "49 Production Place, North Hollywood" };

        Geopoint[] siteCoords = new Geopoint[] {
            new Geopoint(new BasicGeoposition() { Latitude = 41.50395757610948, Longitude = -81.68063198490258 }),
            new Geopoint(new BasicGeoposition() { Latitude = 28.478773, Longitude = -81.467865 }),
            new Geopoint(new BasicGeoposition() { Latitude = 34.139304307493134, Longitude = -118.35425174724723 }) };

        public Page6()
        {
            this.InitializeComponent();

            LocationListView.ItemsSource = siteNames;

            #region Setup the device sizing for the application  
            ApplicationView.GetForCurrentView().TryResizeView(new Size(App.DeviceScreenWidth, App.DeviceScreenHeight));
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(App.DeviceMinimumScreenWidth, App.DeviceMinimumScreenHeight));
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            #endregion

            UpdateBalanceDisplay();
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

        private async void LocationListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if(LocationListView.SelectedItem != null)
            {
                await Map.TrySetSceneAsync(MapScene.CreateFromLocationAndRadius(siteCoords[LocationListView.SelectedIndex], 100));
            }
        }

        private void SatelliteMode_Toggled(object sender, RoutedEventArgs e)
        {
            if (SatelliteMode.IsOn)
                Map.Style = MapStyle.AerialWithRoads;
            else
                Map.Style = MapStyle.Terrain;
        }
    }
}
