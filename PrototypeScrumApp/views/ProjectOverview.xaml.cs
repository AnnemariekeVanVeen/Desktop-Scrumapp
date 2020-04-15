using GalaSoft.MvvmLight.Messaging;
using PrototypeScrumApp.models;
using System.Windows;
using System.Windows.Controls;

namespace PrototypeScrumApp.views
{
    public partial class ProjectOverview : Page
    {
        /// <summary>
        /// Initialize notifications before view
        /// </summary>
        public ProjectOverview()
        {
            InitializeComponent();
            Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);
        }

        /// <summary>
        /// Navigation between pages
        /// with project model as data
        /// </summary>
        /// <param name="obj"> Notification message with data</param>
        private void NotificationMessageReceived(NotificationMessage obj)
        {
            switch (obj.Notification)
            {
                case "ShowProjectPage":
                    {
                        var model = (ProjectModel)obj.Sender;
                        NavigationService?.Navigate(new SingleProjectView(model));
                        break;
                    }
                case "RefreshProjectPage":
                    NavigationService?.Navigate(new ProjectOverview());
                    break;
                case "ShowUpdateProjectPage":
                    {
                        var model = (ProjectModel)obj.Sender;
                        NavigationService?.Navigate(new UpdateProjectView(model));
                        break;
                    }
            }
        }

        /// <summary>
        /// On click button navigate
        /// </summary>
        /// <param name="sender"> Sender</param>
        /// <param name="e"> Routed event arg</param>
        private void CreateProjectButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CreateProjectView());
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
