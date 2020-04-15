using GalaSoft.MvvmLight.Messaging;
using PrototypeScrumApp.viewmodels;
using System.Windows;
using System.Windows.Controls;
using Button = System.Windows.Controls.Button;

namespace PrototypeScrumApp.views
{
    public partial class LoginUsersListView : Page
    {
        /// <summary>
        /// Register notification before
        /// initialization of the view
        /// </summary>
        public LoginUsersListView()
        {
            InitializeComponent();
            Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);
        }

        /// <summary>
        /// Onclick event on register
        /// </summary>
        /// <param name="sender"> Sender</param>
        /// <param name="e"> Routed event arg</param>
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegisterView());
        }

        /// <summary>
        /// Notification for navigation
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
        /// Onchange password box handler
        /// </summary>
        /// <param name="sender"> Sender</param>
        /// <param name="e"> Routed event arg</param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            { ((LoginViewModel)DataContext).Password = ((PasswordBox)sender).Password; }
        }

        /// <summary>
        /// Onclick navigation event
        /// </summary>
        /// <param name="sender"> Sender</param>
        /// <param name="e"> Routed event arg</param>
        private void LoginNewAccountButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new LoginView());
        }

        /// <summary>
        /// PasswordBox visibility handler
        /// </summary>
        /// <param name="sender"> Sender</param>
        /// <param name="e"> Routed event arg</param>
        private void ShowPasswordBox(object sender, RoutedEventArgs e)
        {
            TextBoxPassword.Visibility = Visibility.Visible;
            InputLabelPassword.Visibility = Visibility.Visible;
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
