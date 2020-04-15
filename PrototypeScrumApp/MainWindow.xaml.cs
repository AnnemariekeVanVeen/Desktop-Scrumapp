using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hardcodet.Wpf.TaskbarNotification;
using PrototypeScrumApp.models;
using PrototypeScrumApp.Properties;
using PrototypeScrumApp.views;
using Application = System.Windows.Application;
using MenuItem = System.Windows.Controls.MenuItem;
using MessageBox = System.Windows.MessageBox;

namespace PrototypeScrumApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> _tokens = new List<string>();
        public MainWindow()
        {
            InitializeComponent();

            using (var db = new DatabaseModel())
            {
                db.SaveChanges();
            }

            FrameWithinGrid.Navigate(new HomeView());
        }

        /// <summary>
        /// Switch statement to handle system tray menu
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">routedeventargs e</param>
        private void SystrayMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem) sender;
            string name = menuItem.Name;

            switch (name)
            {
                // Go to home page
                case "HomePageSystray":
                    FrameWithinGrid.Navigate(new HomeView());
                    break;
                // Open window
                case "OpenSystray":
                    if (this.WindowState != WindowState.Normal && this.WindowState != WindowState.Maximized)
                    {
                        this.WindowState = WindowState.Normal;
                    }
                    else if (this.WindowState == WindowState.Maximized)
                    {
                        this.WindowState = WindowState.Maximized;
                        ShowStandardBalloon("Scrumium", "Application is already open");
                    }
                    else
                    {
                        ShowStandardBalloon("Scrumium", "Application is already open");
                    }
                    break;
                // Minimize window
                case "MinimizeSystray":
                    if (this.WindowState == WindowState.Minimized)
                    {
                        ShowStandardBalloon("Scrumium", "Application is already minimized");
                    }
                    else
                    {
                        this.WindowState = WindowState.Minimized;
                    }
                    break;
                // Call async function to close window on message box yes or no
                case "ExitSystray":
                    CloseWindowSystray("Are you sure you want to exit?", "Exiting...");
                    break;
            }
        }

        /// <summary>
        /// Function to show a balloon with custom title and text
        /// </summary>
        /// <param name="title">title of the balloon</param>
        /// <param name="text">text of the balloon</param>
        private void ShowStandardBalloon(string title, string text)
        {
            //show balloon
            NotifyIconPrototype.ShowBalloonTip(title, text, BalloonIcon.Info);
        }

        /// <summary>
        /// Async function to close the window if message box answer is yes
        /// </summary>
        /// <param name="question">message box question</param>
        /// <param name="caption">message box caption</param>
        public async void CloseWindowSystray(string question, string caption)
        {
            bool res = await Task<bool>.Factory.StartNew(() =>
            {
                MessageBoxResult result = MessageBox.Show(question, caption, MessageBoxButton.YesNo);
                return result == MessageBoxResult.Yes;
            });

            if (res.ToString() == "True")
            {
                Close();
            }
        }
    }
}
