using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Controls;

namespace PrototypeScrumApp.views
{
    public partial class LoginView : Page
    {
        /// <summary>
        /// Initialize notifications before view
        /// </summary>
        public LoginView()
        {
            InitializeComponent();
            Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);

            if (Properties.Settings.Default.db_status == "NOK")
            {
                ShowListAccountsButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// Onclick register navigation 
        /// </summary>
        /// <param name="sender"> Sender</param>
        /// <param name="e"> Routed event arg</param>
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegisterView());
        }

        /// <summary>
        /// On login success redirect notification
        /// </summary>
        /// <param name="obj"> Notification message</param>
        private void NotificationMessageReceived(NotificationMessage obj)
        {
            if (obj.Notification == "LoginSuccess")
            {
                NavigationService?.Navigate(new ProjectOverview());
            }
        }

        /// <summary>
        /// On click to show accounts
        /// </summary>
        /// <param name="sender"> Sender</param>
        /// <param name="e"> Routed event arg</param>
        private void ShowListAccountsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new LoginUsersListView());
        }

        /// <summary>
        /// Password box handler
        /// </summary>
        /// <param name="sender"> Sender</param>
        /// <param name="e"> Routed event arg</param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            { ((dynamic)DataContext).Password = ((PasswordBox)sender).Password; }
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
            }
        }
    }
}
