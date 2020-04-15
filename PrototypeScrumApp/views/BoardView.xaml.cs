using GalaSoft.MvvmLight.Messaging;
using PrototypeScrumApp.models;
using PrototypeScrumApp.viewmodels;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataFormats = System.Windows.DataFormats;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using ListBox = System.Windows.Controls.ListBox;

namespace PrototypeScrumApp.views
{
    public partial class BoardView : Page
    {
        private readonly ProjectModel _project;
        private ListBox _dragSource;

        /// <summary>
        /// Constructor to initialize data
        /// before initialize the view
        /// </summary>
        /// <param name="model"> Project model that the data is from</param>
        public BoardView(ProjectModel model)
        {
            InitializeComponent();
            ListBox1.AllowDrop = true;
            ListBox2.AllowDrop = true;

            // Check dataContext for certain view model
            if (!(DataContext is BoardViewModel viewModel)) return;

            // Set project slug to get the project
            viewModel.ProjectSlug = model.ProjectSlug;
            viewModel.ProjectTitle = model.Title;
            _project = model;

            viewModel.GetAllSprintsFromApi();

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
        /// Listbox on mouse down
        /// </summary>
        /// <param name="sender"> Sender</param>
        /// <param name="e"> Mouse button event</param>
        private void ListBox_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var lb = (ListBox)sender;
            if (lb == null || lb.SelectedIndex < 0) return;
            _dragSource = lb;
            DragDrop.DoDragDrop(lb, lb.Tag.ToString(), DragDropEffects.All);
        }

        /// <summary>
        /// Listbox on drop
        /// </summary>
        /// <param name="sender"> Sender</param>
        /// <param name="e"> Drag event </param>
        private void ListBox_OnDrop(object sender, DragEventArgs e)
        {
            var lb = (ListBox)sender;
            if (_dragSource == null || _dragSource.SelectedIndex < 0 ||
                !e.Data.GetDataPresent(DataFormats.StringFormat)) return;
            var item = (BacklogItemModel)_dragSource.SelectedItem;

            item.StateId = Convert.ToInt32(lb.Tag.ToString());

            if (this.DataContext is BoardViewModel boardViewModel)
            {
                boardViewModel.ItemSelected = item;
                boardViewModel.UpdateState.Execute(null);
            }

            ((IList)_dragSource.ItemsSource).Remove(item);
            ((IList)lb.ItemsSource).Add(item);
        }

        //private void UIElement_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        // BoardViewModel boardViewModel = new BoardViewModel();
        //}
    }
}
