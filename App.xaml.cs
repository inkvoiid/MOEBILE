using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace PhoneTemplate
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        public static double Balance {get; set; }
        public static double diceGameProfit = 0;
        public static double slotsProfit = 0;

        public static ContentDialog noMoney = new ContentDialog() { Title = "Out of Money", Content = "Go to your wallet?", CloseButtonText = "No", PrimaryButtonText = "Yes" };
        public static double diceGameMultiplier = 1;

        // Dice Game Logic to prevent games being reset mid game
        public static bool diceGameIsPlaying = false;
        public static int[] diceGameMoeDiceNumbers = new int[5] { 1, 1, 1, 1, 1 };
        public static int[] diceGamePlayerDiceNumbers = new int[5] { 1, 1, 1, 1, 1 };
        public static bool[] diceGameMoeDiceHeld = new bool[5] { false, false, false, false, false };
        public static bool[] diceGamePlayerDiceHeld = new bool[5] { false, false, false, false, false };

        // ----- Sets up the size of the devices -----

        // Minimum Size
        internal static double DeviceMinimumScreenWidth = 200;
        internal static double DeviceMinimumScreenHeight = 400;


        // Phone 5
        internal static double DeviceScreenWidth = 368;
        internal static double DeviceScreenHeight = 636;

        // Phone 6
        //internal static double DeviceScreenWidth = 460;
        //internal static double DeviceScreenHeight = 768;

        // Tablet 8
        //internal static double DeviceScreenWidth = 1038;
        //internal static double DeviceScreenHeight = 636;

        // Desktop 13.3
        //internal static double DeviceScreenWidth = 1288;
        //internal static double DeviceScreenHeight = 715;

        // Xbox 42
        //internal static double DeviceScreenWidth = 976;
        //internal static double DeviceScreenHeight = 538;

        // --------------------------------------------


        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Balance = 0;
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
