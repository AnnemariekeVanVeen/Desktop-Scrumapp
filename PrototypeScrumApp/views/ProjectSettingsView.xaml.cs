using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using PrototypeScrumApp.models;
using PrototypeScrumApp.viewmodels;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PrototypeScrumApp.views
{
    /// <summary>
    /// Interaction logic for ProjectSettingsView.xaml
    /// </summary>
    public partial class ProjectSettingsView : Page
    {
        private readonly ProjectModel _project;
        public ProjectSettingsView(ProjectModel model)
        {
            InitializeComponent();

            // Check dataContext for certain view model
            if (!(this.DataContext is ProjectSettingsViewModel viewModel)) return;

            // Set project slug to get the project
            viewModel.ProjectSlug = model.ProjectSlug;
            _project = model;

            viewModel.GetProjectFromApi();
            viewModel.GetOpenProjectSettings();

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

        /// <summary>
        /// Function to open file explorer and set the chosen file path in the viewmodel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddFilePath(object sender, RoutedEventArgs e)
        {
            string filePath = "";
            OpenFileDialog fileExplorer = new OpenFileDialog
            {
                Filter = "All Files (*.sln*)|*.sln*",
                FilterIndex = 1,
                Multiselect = false
            };

            if (fileExplorer.ShowDialog() == true)
            {
                filePath = fileExplorer.FileName;
                FileInfo fi = new FileInfo(filePath);

                // Check if file extension is .sln
                if (!fi.Extension.Equals(".sln"))
                {
                    MessageBox.Show("Please select a .sln file");
                }
                else if (fi.Extension.Equals(".sln"))
                {
                    ((ProjectSettingsViewModel)DataContext).FilePath = (string)filePath;
                    FilePathTextBox.Text = filePath;
                }
                else
                {
                    MessageBox.Show("Something went wrong");
                }
            }
        }

        /// <summary>
        /// If selection is changed, call the AddIde function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            AddIde();
        }

        /// <summary>
        /// Set the IDE in the viewmodel to the selected item of the combobox
        /// </summary>
        private void AddIde()
        {
            var ideSelected = IdeSelectedComboBox.SelectedItem.ToString()
                .Split(new string[] { ": " }, StringSplitOptions.None).Last();
            switch (ideSelected)
            {
                case "Visual Studio":
                    string ideExec = "devenv.exe";
                    ((ProjectSettingsViewModel)DataContext).IdeExec = (string)ideExec;
                    break;
            }
        }
    }
}
