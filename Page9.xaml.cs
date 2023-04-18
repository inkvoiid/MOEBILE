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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PhoneTemplate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page9 : Page
    {
        private string[] profitsDict;

        public Page9()
        {
            this.InitializeComponent();

            UpdateBalanceDisplay();
            UpdateProfitsDisplay();

            ListViewProfitSources.ItemsSource = profitsDict;
        }

        private void UpdateBalanceDisplay()
        {
            TextBlockBalance.Text = "Balance: $" + App.Balance;
        }

        private void UpdateProfitsDisplay()
        {
            profitsDict = new string[] {
            "Dicey Sixes: $" + App.diceGameProfit, "Lotsa Slots: $" + App.slotsProfit };
        }

        private void AddMoney_Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Page10));
        }

        private void ListViewProfitSources_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ListViewProfitSources.SelectedValue != null)
            {
                if(ListViewProfitSources.SelectedIndex == 0)
                {
                    Frame.Navigate(typeof(Page7));
                }
                else if(ListViewProfitSources.SelectedIndex == 1)
                {
                    Frame.Navigate(typeof(Page3));
                }
            }
        }
    }
}
