using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace EnelSmartInfo
{
    public partial class LoginPage : PhoneApplicationPage
    {
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        public LoginPage()
        {
            InitializeComponent();
        }

        private void Login_Open(object sender, RoutedEventArgs e)
        {
            this.box_popup.IsPopupOpen = true;
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string v_email = box_email.Text;
            string v_password = box_password.Password;

            if ((v_email == "") || (v_password == "")) 
            {
                MessageBox.Show("Compila tutti i campi richiesti.");
            } 
            else
            {
#if DEBUG
                try
                {
                    await Windows.Phone.Speech.VoiceCommands.VoiceCommandService.InstallCommandSetsFromFileAsync(new Uri("ms-appx:///EnelVoiceCommands.xml"));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException);
                }
#endif

                NavigationService.Navigate(new Uri("/HomePage.xaml", UriKind.Relative));
            }
        }
    }
}