using GalaSoft.MvvmLight.Messaging;
using PrototypeScrumApp.helpers;
using PrototypeScrumApp.models;
using PrototypeScrumApp.viewmodels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PrototypeScrumApp.views
{
    public partial class SingleSprintView : Page
    {
        private readonly ProjectModel _project;
        private DateTimeHandler _dateTimeHandler;
        private readonly SprintModel _sprint;

        /// <summary>
        /// Constructor to initialize data
        /// before initialize the view
        /// </summary>
        /// <param name="model"> Project model where backlog items are from</param>
        public SingleSprintView(SprintModel model)
        {
            InitializeComponent();

            // Check dataContext for certain view model
            if (!(DataContext is SingleSprintViewModel viewModel)) return;

            // Set project slug to get the project
            viewModel.ProjectSlug = model.ProjectSlug;
            viewModel.SprintSlug = model.SprintSlug;

            _project = new ProjectModel { ProjectSlug = model.ProjectSlug };
            _sprint = new SprintModel();
            _sprint = model;

            viewModel.GetSprintFromApi();
            viewModel.GetSprintBacklogItemsFromApi();
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
                case "ShowUpdateBacklogItemsPage":
                    var backlogItem = (BacklogItemModel)obj.Sender;
                    NavigationService?.Navigate(new UpdateBacklogItem(backlogItem));
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
        /// Validator for datepicker
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">DatePickerDateValidationErrorEventArgs e</param>
        private void DatePicker_DateValidationError(object sender,
            DatePickerDateValidationErrorEventArgs e)
        {
            var datePickerObj = sender as DatePicker;

            if (DateTime.TryParse(e.Text, out var newDate))
            {
                if (datePickerObj != null && datePickerObj.BlackoutDates.Contains(newDate))
                {
                    MessageBox.Show($"The date, {e.Text}, cannot be selected.");
                }
            }
            else
            {
                e.ThrowException = true;
            }
        }

        /// <summary>
        /// Convert deadline from DateTime to string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConvertStartDate(object sender, RoutedEventArgs e)
        {
            _dateTimeHandler = new DateTimeHandler();
            var a = _dateTimeHandler.Convert(((DatePicker)sender).SelectedDate);
            ((SingleSprintViewModel)DataContext).Sprint[0].StartDate = (string)a;
        }

        /// <summary>
        /// Convert deadline from DateTime to string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConvertEndDate(object sender, RoutedEventArgs e)
        {
            _dateTimeHandler = new DateTimeHandler();
            var a = _dateTimeHandler.Convert(((DatePicker)sender).SelectedDate);
            ((SingleSprintViewModel)DataContext).Sprint[0].EndDate = (string)a;
        }

        /// <summary>
        /// Set blackout dates when DatePicker is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndDate_OnLoaded(object sender, RoutedEventArgs e)
        {
            var dp = sender as DatePicker;
            dp?.BlackoutDates.Add(new CalendarDateRange(new DateTime(0001, 1, 1), DateTime.Today.AddDays(-1)));
            if (dp != null) dp.Text = _sprint.EndDate;
        }

        /// <summary>
        /// Set blackout dates when DatePicker is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartDate_OnLoaded(object sender, RoutedEventArgs e)
        {
            var dp = sender as DatePicker;
            dp?.BlackoutDates.Add(new CalendarDateRange(new DateTime(0001, 1, 1), DateTime.Today.AddDays(-1)));
            if (dp != null) dp.Text = _sprint.StartDate;
        }
    }
}
