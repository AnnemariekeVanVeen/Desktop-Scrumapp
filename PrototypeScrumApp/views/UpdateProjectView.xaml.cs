using GalaSoft.MvvmLight.Messaging;
using PrototypeScrumApp.helpers;
using PrototypeScrumApp.models;
using PrototypeScrumApp.viewmodels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PrototypeScrumApp.views
{
    public partial class UpdateProjectView : Page
    {
        private DateTimeHandler _dateTimeHandler;

        /// <summary>
        /// Constructor to initialize data
        /// before initialize the view
        /// </summary>
        /// <param name="model"> Project model where backlog items are from</param>
        public UpdateProjectView(ProjectModel model)
        {
            InitializeComponent();

            // Check dataContext for certain view model
            if (!(DataContext is UpdateProjectViewModel viewModel)) return;

            // Set project slug to get the project
            viewModel.ProjectSlug = model.ProjectSlug;

            // Call the apis
            viewModel.GetProjectFromApi();
            viewModel.GetAllUsersFromApi();
            // Compare collections to set checkbox value
            viewModel.CompareCollections();

            Messenger.Default.Register<NotificationMessage>(this, NavigateToNewPageNotification);
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
        /// Navigation between pages
        /// with project model as data
        /// </summary>
        /// <param name="obj"> Notification message with data</param>
        private void NavigateToNewPageNotification(NotificationMessage obj)
        {
            switch (obj.Notification)
            {
                case "ShowProjectOverview":
                    NavigationService?.Navigate(new ProjectOverview());
                    break;
                case "ShowUpdateProjectPage":
                    var model = (ProjectModel)obj.Sender;
                    NavigationService?.Navigate(new UpdateProjectView(model));
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
        private void ConvertDeadlineDate(object sender, RoutedEventArgs e)
        {
            _dateTimeHandler = new DateTimeHandler();
            var a = _dateTimeHandler.Convert(((DatePicker)sender).SelectedDate);
            ((UpdateProjectViewModel)DataContext).Deadline = (string)a;
        }

        /// <summary>
        /// Set blackout dates when DatePicker is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Date_OnLoaded(object sender, RoutedEventArgs e)
        {
            var dp = sender as DatePicker;
            dp?.BlackoutDates.Add(new CalendarDateRange(new DateTime(0001, 1, 1), DateTime.Today.AddDays(-1)));
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
            var cv = CollectionViewSource.GetDefaultView(UsersList.ItemsSource);
            if (filter == "")
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    UserModel item = o as UserModel;
                    if (t.Name == "txtId")
                        return item != null && (item.Id == Convert.ToInt32(filter));
                    return item != null && (item.Name.ToUpper().StartsWith(filter.ToUpper()));
                };
            }
        }
    }
}
