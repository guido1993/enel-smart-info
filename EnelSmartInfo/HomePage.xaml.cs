using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using EnelSmartInfo.Resources;
using Microsoft.Phone.Scheduler;
using System.Windows.Media;
using Microsoft.Phone.Tasks;
using Coding4Fun.Toolkit.Controls;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;

namespace EnelSmartInfo
{
    public partial class HomePage : PhoneApplicationPage
    {
        public HomePage()
        {
            InitializeComponent();

            ApplicationBar = new ApplicationBar();
            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            ApplicationBar.Mode = ApplicationBarMode.Minimized;

            ApplicationBarIconButton button1 = new ApplicationBarIconButton();
            button1.IconUri = new Uri("/Assets/Icons/questionmark.png", UriKind.Relative);
            button1.Text = AppResources.About;
            button1.Click += new EventHandler(About_Click);
            ApplicationBar.Buttons.Add(button1);

            ApplicationBarMenuItem menuItem1 = new ApplicationBarMenuItem("logout");
            menuItem1.Click += new EventHandler(Logout_Click);

            ApplicationBar.MenuItems.Add(menuItem1);

            box_main.Visibility = System.Windows.Visibility.Visible;
            box_main_animation.Begin();

        }

        protected virtual void OnNavigatedTo(NavigationEventArgs e)
        {
            string id = NavigationContext.QueryString["id"];
            base.OnNavigatedTo(e);

            if (id != null)
            {
                if (NavigationService.CanGoBack) { while (this.NavigationService.BackStack.Any()) { this.NavigationService.RemoveBackEntry(); } }
            }

            string v_test = "";

            try
            {
                v_test= IsolatedStorageSettings.ApplicationSettings["test"].ToString();
            }
            catch {

                v_test = "1";

                IsolatedStorageSettings.ApplicationSettings["test"] = "1";
                IsolatedStorageSettings.ApplicationSettings.Save();
            }

            if (v_test == "1") box_image.Source = new BitmapImage(new Uri("/Assets/Icons/1.png", UriKind.RelativeOrAbsolute));
            if (v_test == "2") box_image.Source = new BitmapImage(new Uri("/Assets/Icons/2.png", UriKind.RelativeOrAbsolute));
            if (v_test == "3") box_image.Source = new BitmapImage(new Uri("/Assets/Icons/3.png", UriKind.RelativeOrAbsolute));
            if (v_test == "4") box_image.Source = new BitmapImage(new Uri("/Assets/Icons/4.png", UriKind.RelativeOrAbsolute));

            
        }


        private void About_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/About.xaml", UriKind.Relative));
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Test.xaml", UriKind.Relative));
        }

        private void Click_Background_Remove(object sender, RoutedEventArgs e)
        {
            try
            {
                ScheduledActionService.Remove("PeriodicTaskDemo");
            }
            catch { }

            MessageBox.Show("Hai disabilitato le notifiche push");
        }

        private void Click_Background_Test(object sender, RoutedEventArgs e)
        {
            var toast = new ToastPrompt
            {
                Title = "ENEL smart info",
                Message = "Horizontal text",
                TextOrientation = System.Windows.Controls.Orientation.Horizontal,
                ImageSource = new BitmapImage(new Uri("/Assets/ApplicationIcon.png", UriKind.RelativeOrAbsolute)),
                ImageWidth = 26,
                Stretch = Stretch.Uniform
            };

            toast.Show();
        }

        private void Click_Background(object sender, RoutedEventArgs e)
        {

            PeriodicTask periodicTask = new PeriodicTask("PeriodicTaskDemo");
            periodicTask.Description = "info del background";
            try
            {
                ScheduledActionService.Add(periodicTask);

#if DEBUG_AGENT
  ScheduledActionService.LaunchForTest(periodicTask, TimeSpan.FromSeconds(1));
#endif

                string return_message = "hai attivato le notifiche push";
                MessageBox.Show(return_message);
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message.Contains("exists already"))
                {
                    string return_message = "son già attive le notifiche push";
                    MessageBox.Show(return_message);
                }
                if (exception.Message.Contains("BNS Error: The action is disabled"))
                {
                    string return_message = "impossibile avviare il servizio";
                    MessageBox.Show(return_message);
                }
            }
            catch (SchedulerServiceException)
            {

            }
        }

        private void One_Day_Click(object sender, RoutedEventArgs e)
        {
            box_kw_title.Text = "GIORNALIERO";
            box_kw_value.Text = "10";
            this.box_popup.IsPopupOpen = false;
        }

        private void One_Month_Click(object sender, RoutedEventArgs e)
        {
            box_kw_title.Text = "ULTIMO MESE";
            box_kw_value.Text = "100";
            this.box_popup.IsPopupOpen = false;
        }

        private void Two_Month_Click(object sender, RoutedEventArgs e)
        {
            box_kw_title.Text = "ULTIMI DUE MESI";
            box_kw_value.Text = "200";
            this.box_popup.IsPopupOpen = false;
        }

        private void One_Year_Click(object sender, RoutedEventArgs e)
        {
            box_kw_title.Text = "ULTIMO ANNO";
            box_kw_value.Text = "1200";
            this.box_popup.IsPopupOpen = false;
        }

        private void Border_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.box_popup.IsPopupOpen = true;
        }

        private void box_border_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            box_border.Foreground = new SolidColorBrush(Colors.White);
        }

        private void box_border_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            box_border.Foreground = new SolidColorBrush(Colors.Orange);
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.box1.IsPopupOpen = true;
        }

        private void TextBlock_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.box2.IsPopupOpen = true;
        }

        private void TextBlock_Tap_2(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.box3.IsPopupOpen = true;
        }

        private void TextBlock_Tap_3(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.box4.IsPopupOpen = true;
        }

        private void TextBlock_Tap_4(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.box5.IsPopupOpen = true;
        }

        private void TextBlock_Tap_5(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://www.enel.it/", UriKind.Absolute);
            webBrowserTask.Show();
        }

    }
}