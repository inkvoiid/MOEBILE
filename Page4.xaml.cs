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
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PhoneTemplate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page4 : Page
    {
        private ContentDialog rules = new ContentDialog()
        {
            Title = "Lottery",
            Content = "Select the number of ticket rows to buy, then tap Get Ticket to generate a ticket\n\nTicket rows cost $2.50 each",
            PrimaryButtonText = "OK"
        };

        int[] lottoNums;
        Random rand;
        int rows;

        public Page4()
        {
            this.InitializeComponent();

            #region Setup the device sizing for the application  
            ApplicationView.GetForCurrentView().TryResizeView(new Size(App.DeviceScreenWidth, App.DeviceScreenHeight));
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(App.DeviceMinimumScreenWidth, App.DeviceMinimumScreenHeight));
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            #endregion

            UpdateBalanceDisplay();

            lottoNums = new int[6] { 0, 0, 0, 0, 0, 0 };
            rand = new Random();
            if(!int.TryParse(TextBoxTickets.Text, out rows))
            {
                TextBoxTickets.Text = "1";
                rows = 1;
            }


            TextBlockTicket.Text = "*-------- Lottery Ticket --------*";
            TextBlockTicketDropShadow.Text = "\n";
            SetLength();

            DisplayRules();
        }

        private async void DisplayRules()
        {
            rules.ShowAsync();
        }

        // Careful, this check for poor has extra code becuase it's only called from one part of the code
        private async void CheckForPoor()
        {
            //Make sure the ticket count is accurate
            if (!int.TryParse(TextBoxTickets.Text, out rows))
            {
                TextBoxTickets.Text = "0";
                rows = 0;
            }
            if (Convert.ToInt32(TextBoxTickets.Text) > 20)
            {
                TextBoxTickets.Text = "20";
                rows = 20;
            }


            if (App.Balance < 1)
            {

                ContentDialogResult gotoWallet = await App.noMoney.ShowAsync();
                if (gotoWallet == ContentDialogResult.Primary)
                {
                    Frame.Navigate(typeof(Page9));
                }
            }
            else if (App.Balance < (2.50 * rows))
            {
                ContentDialog notEnoughMoney = new ContentDialog()
                {
                    Title = "Not Enough Money",
                    Content = rows + " ticket rows costs $" + (2.50 * rows) + " to play.\nYou don't have enough money for that number of ticket rows.\n\nEither lower the number of ticket rows or top up your wallet.",
                    CloseButtonText = "Back",
                    PrimaryButtonText = "Go to Wallet"
                };
                ContentDialogResult gotoWallet = await notEnoughMoney.ShowAsync();
                if (gotoWallet == ContentDialogResult.Primary)
                {
                    Frame.Navigate(typeof(Page9));
                }
            }
            else
            {
                GenerateTicket();
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

        private void GenerateNumbers()
        {
            lottoNums = new int[6] { 0, 0, 0, 0, 0, 0 };
            for (int col = 0; col < lottoNums.Length; col++)
            {
                lottoNums[col] = rand.Next(1, 50);
                for (int check = 0; check < lottoNums.Length; check++)
                {
                    if ((lottoNums[col] == lottoNums[check]) && (col != check))
                    {
                        col--;
                        break;
                    }
                }
            }
        }

        private void BubbleSort(ref int[] array)
        {
            int tempInt;
            for (int i = 0; i < array.Length-1; i++)
            {
                bool hasChanged = false;
                for (int j = 0; j < (array.Length-1)- i; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        hasChanged = true;
                        tempInt = array[j + 1];
                        array[j + 1] = array[j];
                        array[j] = tempInt;
                    }
                }
                if (!hasChanged)
                {
                    break;
                }
            }
        }

        private void PrintNumbers()
        {
            TextBlockTicket.Text += "|  ";
            for (int i = 0; i < lottoNums.Length; i++)
            {
                if(i == 0)
                {
                    if (lottoNums[0] < 10)
                    {
                        TextBlockTicket.Text += "  " + lottoNums[0];
                    }
                    else
                    {
                        TextBlockTicket.Text += lottoNums[0];
                    }
                }
                else
                {
                    if (lottoNums[i] < 10)
                    {
                        TextBlockTicket.Text += "      " + lottoNums[i];
                    }
                    else
                    {
                        TextBlockTicket.Text += "    " + lottoNums[i];
                    }
                }
            }

            TextBlockTicketDropShadow.Text += "\n";
        }


        #endregion

        private void GenerateTicket()
        {
            App.Balance -= (2.50 * rows);
            UpdateBalanceDisplay();
            TextBlockTicket.Text = "*-------- Lottery Ticket --------*\n";
            TextBlockTicketDropShadow.Text = "\n";
            if (!int.TryParse(TextBoxTickets.Text, out rows))
            {
                TextBoxTickets.Text = "0";
                rows = 0;
            }
            if (Convert.ToInt32(TextBoxTickets.Text) > 20)
            {
                TextBoxTickets.Text = "20";
                rows = 20;
            }
            for (int i = 0; i < rows; i++)
            {
                GenerateNumbers();
                BubbleSort(ref lottoNums);
                PrintNumbers();
                TextBlockTicket.Text += "  |\n";
            }
            TextBlockTicket.Text += "*----------------------------------*";
            TextBlockTicketDropShadow.Text += "\n";

            SetLength();
        }

        private void ButtonSelect_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CheckForPoor();
        }

        private void SetLength()
        {
            TicketBorder.Height = Double.NaN;

            TicketDropShadow.Height = Double.NaN;
        }
    }
}
