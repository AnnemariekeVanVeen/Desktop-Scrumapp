using GalaSoft.MvvmLight.Messaging;
using PrototypeScrumApp.models;
using PrototypeScrumApp.viewmodels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PrototypeScrumApp.views
{
    public partial class BacklogItemView : Page
    {
        private readonly ProjectModel _project;

        /// <summary>
        /// Constructor to initialize data
        /// before initialize the view
        /// </summary>
        /// <param name="model"> Project model where backlog items are from</param>
        public BacklogItemView(ProjectModel model)
        {
            InitializeComponent();

            // Check dataContext for certain view model
            if (!(DataContext is BacklogItemViewModel viewModel)) return;

            // Set project slug to get the project
            viewModel.ProjectSlug = model.ProjectSlug;
            _project = model;

            // Call the apis
            viewModel.GetAllBacklogItemsFromApi();
            viewModel.GetProjectFromApi();
            viewModel.GetBacklogTypesFromApi();

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
        /// SearchBox for the backlog items
        /// </summary>
        /// <param name="sender"> Object sender</param>
        /// <param name="e"> Text changed event</param>
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var t = (TextBox)sender;
            var filter = t.Text;
            var cv = CollectionViewSource.GetDefaultView(BacklogItemsGrid.ItemsSource);
            if (filter == "")
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    var item = o as BacklogItemModel;
                    if (t.Name == "txtId")
                        return item != null && (item.BacklogItemId == Convert.ToInt32(filter));
                    return item != null && (item.Action.ToUpper().StartsWith(filter.ToUpper()));
                };
            }
        }
    }
}
