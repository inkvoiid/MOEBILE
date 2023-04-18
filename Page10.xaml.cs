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
    public sealed partial class Page10 : Page
    {
        public Page10()
        {
            this.InitializeComponent();
            UpdateBalanceDisplay();

            if (TextBoxCardHolder.Text == "" || TextBoxCardNumber.Text == ""
                || TextBoxCardExpiration.Text == "" || TextBoxCardCVC.Text == "")
            {
                ExpanderCardDetails.IsExpanded = true;
                ExpanderTopUpAmount.IsExpanded = false;
            }
            else
            {
                ExpanderCardDetails.IsExpanded = false;
                ExpanderTopUpAmount.IsExpanded = true;
            }
        }

        private void UpdateBalanceDisplay()
        {
            TextBoxBalance.Text = "$" + App.Balance;
        }

        private void ButtonDone_Tapped(object sender, TappedRoutedEventArgs e)
        {
            float topUpAmount;
            if (!float.TryParse(TextBoxTopUpAmount.Text, out topUpAmount))
            {
                TextBoxTopUpAmount.Text = "0.00";
                topUpAmount = 0;
            }
            if (topUpAmount > 0)
            {
                App.Balance += topUpAmount;
                Frame.Navigate(typeof(Page9));
            }
        }

        private void ExpanderCardDetails_Expanding(Microsoft.UI.Xaml.Controls.Expander sender, Microsoft.UI.Xaml.Controls.ExpanderExpandingEventArgs args)
        {
            ExpanderTopUpAmount.IsExpanded = false;
        }

        private void ExpanderTopUpAmount_Expanding(Microsoft.UI.Xaml.Controls.Expander sender, Microsoft.UI.Xaml.Controls.ExpanderExpandingEventArgs args)
        {
            ExpanderCardDetails.IsExpanded = false;
        }
    }
}
