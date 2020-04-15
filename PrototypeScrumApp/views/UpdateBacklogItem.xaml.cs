using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using PrototypeScrumApp.models;
using PrototypeScrumApp.viewmodels;

namespace PrototypeScrumApp.views
{
    public partial class UpdateBacklogItem : Page
    {
        private readonly ProjectModel _project;

        public UpdateBacklogItem(BacklogItemModel backlogItemModel)
        {
            InitializeComponent();

            // Check dataContext for certain view model
            if (!(DataContext is SingleBacklogItemViewModel viewModel)) return;

            // Set project slug to get the project
            viewModel.ProjectSlug = backlogItemModel.ProjectSlug;
            viewModel.BacklogItemId = backlogItemModel.BacklogItemId;

            _project = new ProjectModel { ProjectSlug = backlogItemModel.ProjectSlug };

            viewModel.GetBacklogItemFromApi();
            viewModel.GetAllBacklogItemsFromApi();
            viewModel.GetBacklogTypesFromApi();

            // Disable settings button if database installation failed
            if (Properties.Settings.Default.db_status == "NOK")
            {
                //ProjectSettingsButton.IsEnabled = false;
            }

            Messenger.Default.Register<NotificationMessage>(this, NavigateToNewPageNotification);
        }

        /// <summary>
        /// Navigation between pages
        /// with project model as data
        /// </summary>
        /// <param name="obj"> Notification message with data</param>
        private void NavigateToNewPageNotification(NotificationMessage obj)
        {
            switch (obj.Notification)
            {
                case "ShowBacklogItemsPage":
                    this.NavigationService?.Navigate(new BacklogItemView(_project));
                    break;
                case "ShowUpdateBacklogItemsPage":
                    var model = (BacklogItemModel)obj.Sender;
                    NavigationService?.Navigate(new UpdateBacklogItem(model));
                    break;
                case "ShowSprintsPage":
                    this.NavigationService?.Navigate(new SprintView(_project));
                    break;
                case "ShowSingleProjectPage":
                    this.NavigationService?.Navigate(new SingleProjectView(_project));
                    break;
                case "ShowBoardPage":
                    this.NavigationService?.Navigate(new BoardView(_project));
                    break;
                case "CreateBacklogItemPage":
                    this.NavigationService?.Navigate(new CreateBacklogItemView(_project));
                    break;
                case "ShowProjectSettingsPage":
                    NavigationService?.Navigate(new ProjectSettingsView(_project));
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
            Button btn = (Button)sender;
            string name = btn.Name;

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
