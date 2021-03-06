﻿using GalaSoft.MvvmLight.Messaging;
using PrototypeScrumApp.viewmodels;
using System.Windows;
using System.Windows.Controls;

namespace PrototypeScrumApp.views
{
    public partial class UserView : Page
    {
        /// <summary>
        /// Constructor to initialize data
        /// before initialize the view
        /// </summary>
        public UserView()
        {
            InitializeComponent();

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
                case "ShowUserPage":
                    NavigationService?.Navigate(new UserView());
                    break;
            }
        }


        /// <summary>
        /// Password box handler
        /// </summary>
        /// <param name="sender"> Sender</param>
        /// <param name="e"> Routed event arg</param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            { ((UserViewModel)DataContext).Password = ((PasswordBox)sender).Password; }
        }

        /// <summary>
        /// Repeat password box handler
        /// </summary>
        /// <param name="sender"> Sender</param>
        /// <param name="e"> Routed event arg</param>
        private void RepeatPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            { ((UserViewModel)DataContext).RepeatPassword = ((PasswordBox)sender).Password; }
        }
    }
}
