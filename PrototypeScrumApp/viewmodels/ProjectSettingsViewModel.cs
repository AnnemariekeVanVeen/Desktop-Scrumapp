using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using PrototypeScrumApp.Annotations;
using PrototypeScrumApp.models;
using PrototypeScrumApp.repositories;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace PrototypeScrumApp.viewmodels
{
    public class ProjectSettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ProjectRepository _projectRepository;

        private string _filePath;
        private string _ideExec;

        [JsonProperty("project_title")]
        private string _projectTitle;

        [JsonProperty("project_slug")]
        private string _projectSlug;

        public ProjectSettingsViewModel()
        {
            // Set the repositories
            _projectRepository = new ProjectRepository();

            // Make observable collections
            SingleProject = new ObservableCollection<SingleProjectModel>();
            ProjectSettings = new ObservableCollection<IdeModel>();

            // Make commands
            MakeBacklogItemsCommand();
            MakeShowSprintsCommand();
            MakeShowBoardCommand();
            MakeShowSingleProjectCommand();
            MakeUpdateProjectSettingsCommand();
            MakeOpenProjectCommand();
            MakeShowProjectSettingsCommand();
        }

        public ICommand ShowSprints { get; set; }
        public ICommand ShowBacklogItems { get; set; }
        public ICommand ShowBoardPage { get; set; }
        public ICommand ShowSingleProject { get; set; }
        public ICommand UpdateProjectSettings { get; set; }
        public ICommand OpenProject { get; set; }
        public ICommand ShowProjectSettings { get; set; }

        public ObservableCollection<SingleProjectModel> SingleProject { get; set; }

        public ObservableCollection<IdeModel> ProjectSettings { get; set; }

        public string ProjectTitle
        {
            get => _projectTitle;
            set
            {
                _projectTitle = value;
                OnPropertyChanged();
            }
        }

        public string ProjectSlug
        {
            get => _projectSlug;
            set
            {
                _projectSlug = value;
                OnPropertyChanged();
            }
        }

        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged();
            }
        }

        public string IdeExec
        {
            get => _ideExec;
            set
            {
                _ideExec = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Get the project from the api call and set it in the collections
        /// </summary>
        public void GetProjectFromApi()
        {
            var project = _projectRepository.GetOneProject(ProjectSlug);

            if (project == null) return;

            SingleProject.Add(project);
            ProjectTitle = project.Title;
        }

        /// <summary>
        /// Function to retrieve the settings of a project that are necessary for opening the project with an IDE
        /// </summary>
        public void GetOpenProjectSettings()
        {
            var projectSettings = _projectRepository.RetrieveFromDb(SingleProject[0].Id);
            foreach (var setting in projectSettings)
            {
                if (setting.Ide == "devenv.exe")
                {
                    setting.Ide = "Visual Studio";
                }
                ProjectSettings.Add(setting);
            }
        }

        /// <summary>
        /// Make a new command for navigating to Sprints view
        /// </summary>
        private void MakeShowSprintsCommand()
        {
            ShowSprints = new RelayCommand(() =>
            {
                Messenger.Default.Send(new NotificationMessage("ShowSprintsPage"));
            });
        }

        /// <summary>
        /// Make a new command for navigating to backlogItems view
        /// </summary>
        private void MakeBacklogItemsCommand()
        {
            ShowBacklogItems = new RelayCommand(() =>
            {
                Messenger.Default.Send(new NotificationMessage("ShowBacklogItemsPage"));
            });
        }

        /// <summary>
        /// Make a new command for navigating to board view
        /// </summary>
        private void MakeShowBoardCommand()
        {
            ShowBoardPage = new RelayCommand(() =>
            {
                Messenger.Default.Send(new NotificationMessage("ShowBoardPage"));
            });
        }

        /// <summary>
        /// Make a new command for navigating to single project view
        /// </summary>
        private void MakeShowSingleProjectCommand()
        {
            ShowSingleProject = new RelayCommand(() =>
            {
                Messenger.Default.Send(new NotificationMessage("ShowSingleProjectPage"));
            });
        }

        /// <summary>
        /// Make a new command for navigating to project settings view
        /// </summary>
        private void MakeShowProjectSettingsCommand()
        {
            ShowProjectSettings = new RelayCommand(() =>
            {
                Messenger.Default.Send(new NotificationMessage("ShowProjectSettingsPage"));
            });
        }

        /// <summary>
        /// Make a new command for updating the project settings
        /// </summary>
        public void MakeUpdateProjectSettingsCommand()
        {
            UpdateProjectSettings = new RelayCommand(() =>
            {
                _projectRepository.AddSettingsToDb(SingleProject[0].Id, FilePath, IdeExec);

                Messenger.Default.Send(new NotificationMessage("ShowProjectSettingsPage"));
            });
        }

        /// <summary>
        /// Make a new command for opening a project with a chosen IDE
        /// </summary>
        public void MakeOpenProjectCommand()
        {
            OpenProject = new RelayCommand(() =>
            {
                var projectSettings = _projectRepository.RetrieveFromDb(SingleProject[0].Id);
                if (projectSettings[0].Ide != string.Empty && projectSettings[0].FilePath != string.Empty)
                {
                    try
                    {
                        Process.Start(projectSettings[0].Ide, projectSettings[0].FilePath);
                    }
                    catch (Win32Exception e)
                    {
                        Console.WriteLine(e);
                        MessageBox.Show("An error occured with the selected IDE.");
                    }

                }
                else if (projectSettings[0].Ide == string.Empty || projectSettings[0].FilePath == string.Empty)
                {
                    MessageBox.Show("Don't leave fields empty");
                }
            });
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}