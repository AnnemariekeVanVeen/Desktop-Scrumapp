using GalaSoft.MvvmLight.Messaging;
using PrototypeScrumApp.models;
using PrototypeScrumApp.viewmodels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PrototypeScrumApp.views
{
    public partial class SprintView : Page
    {
        private readonly ProjectModel _project;

        /// <summary>
        /// Constructor to initialize data
        /// before initialize the view
        /// </summary>
        /// <param name="model"> Project model where backlog items are from</param>
        public SprintView(ProjectModel model)
        {
            InitializeComponent();

            // Check dataContext for certain view model
            if (!(DataContext is SprintViewModel viewModel)) return;

            // Set project slug to get the project
            viewModel.ProjectSlug = model.ProjectSlug;
            _project = model;

            // Call the apis
            viewModel.GetAllSprintsFromApi();
            viewModel.GetProjectFromApi();

            // Disable settings button if database installation failed
            if (Properties.Settings.Default.db_status == "NOK")
            {
                ProjectSettingsButton.IsEnabled = false;
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
                    NavigationService?.Navigate(new BacklogItemView(_project));
                    break;
                case "ShowSprintsPage":
                    NavigationService?.Navigate(new SprintView(_project));
                    break;
                case "ShowSingleProjectPage":
                    NavigationService?.Navigate(new SingleProjectView(_project));
                    break;
                case "ShowBoardPage":
                    NavigationService?.Navigate(new BoardView(_project));
                    break;
                case "CreateSprintPage":
                    NavigationService?.Navigate(new CreateSprintView(_project));
                    break;
                case "ShowSprintPage":
                    var model = (SprintModel)obj.Sender;
                    NavigationService?.Navigate(new SingleSprintView(model));
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

        /// <summary>
        /// SearchBox event handler
        /// </summary>
        /// <param name="sender"> Sender</param>
        /// <param name="e"> Text changed event arg</param>
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var t = (TextBox)sender;
            var filter = t.Text;
            var cv = CollectionViewSource.GetDefaultView(SprintsGrid.ItemsSource);
            if (filter == "")
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    SprintModel item = o as SprintModel;
                    if (t.Name == "txtId")
                        return item != null && (item.Id == Convert.ToInt32(filter));
                    return item != null && (item.Name.ToUpper().StartsWith(filter.ToUpper()));
                };
            }
        }

    }
}
