using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Button = System.Windows.Controls.Button;

namespace PrototypeScrumApp.views
{
    public partial class HomeView : Page
    {
        /// <summary>
        /// Initialize visibility of buttons 
        /// </summary>
        public HomeView()
        {
            InitializeComponent();

            if (Properties.Settings.Default.active_api_token != string.Empty)
            {
                LoginPageButton.Visibility = Visibility.Hidden;
                ProjectsButton.Visibility = Visibility.Visible;
                UserButton.Visibility = Visibility.Visible;
                SettingsButton.Visibility = Visibility.Visible;
                LogoutButton.Visibility = Visibility.Visible;
            }
            else
            {
                LoginPageButton.Visibility = Visibility.Visible;
                ProjectsButton.Visibility = Visibility.Hidden;
                UserButton.Visibility = Visibility.Hidden;
                SettingsButton.Visibility = Visibility.Hidden;
                LogoutButton.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// The navigation of the side nav
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavigateSideNav(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var name = btn.Name;

            switch (name)
            {
                case "HomePageButton":
                    NavigationService?.Navigate(new HomeView());
                    break;
                case "LoginPageButton":
                    NavigationService?.Navigate(new LoginView());
                    break;
                case "ProjectsButton":
                    NavigationService?.Navigate(new ProjectOverview());
                    break;
                case "UserButton":
                    NavigationService?.Navigate(new UserView());
                    break;
                case "SettingsButton":
                    NavigationService?.Navigate(new SettingsView());
                    break;
                case "LogoutButton":
                    Properties.Settings.Default.active_api_token = string.Empty;
                    NavigationService?.Navigate(new LoginView());
                    break;
            }
        }

        /// <summary>
        /// Hyperlink to requested url
        /// </summary>
        /// <param name="sender"> Sender</param>
        /// <param name="e"> Request navigate event argument</param>
        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
