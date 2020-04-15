using System.Windows;
using System.Windows.Controls;


namespace PrototypeScrumApp.views
{
    public partial class SettingsView : Page
    {
        /// <summary>
        /// Initialize checkboxes and view
        /// </summary>
        public SettingsView()
        {
            InitializeComponent();

            switch (Properties.Settings.Default.db_status)
            {
                case "OK":
                    DatabaseCheckBox.IsChecked = true;
                    break;
                case "NOK":
                    DatabaseCheckBox.IsChecked = false;
                    break;
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
    }
}
