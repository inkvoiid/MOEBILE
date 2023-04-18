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
using Windows.UI.ViewManagement;    // Setup the device sizing for the application

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

#region You do not need to edit this page - it sets up the clock, navigation and device size selection for you

namespace PhoneTemplate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Timer code for the Clock
        DispatcherTimer dispatcherTimer;
        #endregion
        public MainPage()
        {
            this.InitializeComponent();

            #region Setup the device sizing for the application         
            ApplicationView.GetForCurrentView().TryResizeView(new Size(App.DeviceScreenWidth, App.DeviceScreenHeight));
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(App.DeviceMinimumScreenWidth, App.DeviceMinimumScreenHeight));
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            #endregion

            #region Timer code for the Clock
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1);
            dispatcherTimer.Start();
            #endregion

            #region Loading the first of the application
            MyFrame.Navigate(typeof(Page1));
            #endregion
        }
        
        #region Timer code for the Clock
        void dispatcherTimer_Tick(object sender, object e)
        {
            TimeTextBlock.Text = DateTime.Now.ToShortTimeString();
        }
        #endregion

        #region Setup the device sizing for the application  
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ApplicationView.GetForCurrentView().TryResizeView(new Size(App.DeviceScreenWidth, App.DeviceScreenHeight));
        }
        #endregion

        #region Navigation code
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MyFrame.CanGoBack)
            {
                MyFrame.GoBack();
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (MyFrame.CanGoForward)
            {
                MyFrame.GoForward();
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(typeof(Page1));
        }
        #endregion

        #region Device size selection code
        private void Item1_Click(object sender, RoutedEventArgs e)
        {
            // Phone 5
            App.DeviceScreenWidth = 368;
            App.DeviceScreenHeight = 636;
            ApplicationView.GetForCurrentView().TryResizeView(new Size(App.DeviceScreenWidth, App.DeviceScreenHeight));
        }
        private void Item2_Click(object sender, RoutedEventArgs e)
        {
            // Phone 6
            App.DeviceScreenWidth = 460;
            App.DeviceScreenHeight = 768;
            ApplicationView.GetForCurrentView().TryResizeView(new Size(App.DeviceScreenWidth, App.DeviceScreenHeight));
        }
        private void Item3_Click(object sender, RoutedEventArgs e)
        {
            // Tablet 8
            App.DeviceScreenWidth = 1038;
            App.DeviceScreenHeight = 636;
            ApplicationView.GetForCurrentView().TryResizeView(new Size(App.DeviceScreenWidth, App.DeviceScreenHeight));
        }
        private void Item4_Click(object sender, RoutedEventArgs e)
        {
            // Desktop 13.3
            App.DeviceScreenWidth = 1288;
            App.DeviceScreenHeight = 715;
            ApplicationView.GetForCurrentView().TryResizeView(new Size(App.DeviceScreenWidth, App.DeviceScreenHeight));
        }
        private void Item5_Click(object sender, RoutedEventArgs e)
        {
            // Xbox 42
            App.DeviceScreenWidth = 976;
            App.DeviceScreenHeight = 538;
            ApplicationView.GetForCurrentView().TryResizeView(new Size(App.DeviceScreenWidth, App.DeviceScreenHeight));
        }
        #endregion

        //private void CameraButton_Click(object sender, RoutedEventArgs e)
        //{

        //}
    }
}

#endregion
