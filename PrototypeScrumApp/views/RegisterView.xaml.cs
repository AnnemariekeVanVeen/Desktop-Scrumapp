using PrototypeScrumApp.viewmodels;
using System.Windows;
using System.Windows.Controls;

namespace PrototypeScrumApp.views
{
    public partial class RegisterView : Page
    {
        /// <summary>
        /// Initialize view
        /// </summary>
        public RegisterView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Login navigation on click
        /// </summary>
        /// <param name="sender"> Sender</param>
        /// <param name="e"> Routed event arg</param>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new LoginView());
        }

        /// <summary>
        /// Password box handler
        /// </summary>
        /// <param name="sender"> Sender</param>
        /// <param name="e"> Routed event arg</param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            { ((RegisterViewModel)DataContext).Password = ((PasswordBox)sender).Password; }
        }

        /// <summary>
        /// Repeat password box handler
        /// </summary>
        /// <param name="sender"> Sender</param>
        /// <param name="e"> Routed event arg</param>
        private void RepeatPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            { ((RegisterViewModel)DataContext).RepeatPassword = ((PasswordBox)sender).Password; }
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
