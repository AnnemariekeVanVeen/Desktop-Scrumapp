﻿using GalaSoft.MvvmLight.Messaging;
using PrototypeScrumApp.models;
using PrototypeScrumApp.viewmodels;
using System.Windows;
using System.Windows.Controls;

namespace PrototypeScrumApp.views
{
    public partial class SingleProjectView : Page
    {
        private readonly ProjectModel _project;

        /// <summary>
        /// Constructor to initialize data
        /// before initialize the view
        /// </summary>
        /// <param name="model"> Project model where backlog items are from</param>
        public SingleProjectView(ProjectModel model)
        {
            InitializeComponent();

            // Check dataContext for certain view model
            if (!(this.DataContext is SingleProjectViewModel viewModel)) return;

            // Set project slug to get the project
            viewModel.ProjectSlug = model.ProjectSlug;
            _project = model;

            // Call the apis
            viewModel.GetProjectFromApi();
            viewModel.GetAllSprintsFromApi();
            viewModel.GetAllBacklogItemsFromApi();

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

        private void SettingsProject(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ProjectSettingsView(_project));
        }
    }
}
