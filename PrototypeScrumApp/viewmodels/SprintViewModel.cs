using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using PrototypeScrumApp.Annotations;
using PrototypeScrumApp.factories;
using PrototypeScrumApp.models;
using PrototypeScrumApp.repositories;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PrototypeScrumApp.viewmodels
{
    public class SprintViewModel : SprintFactory, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly SprintRepository _sprintRepository;
        private readonly ProjectRepository _projectRepository;

        [JsonProperty("name")]
        private string _name;

        [JsonProperty("project_slug")]
        private string _projectSlug;

        [JsonProperty("project_title")]
        private string _projectTitle;

        public SprintViewModel()
        {
            // Set sprint repository
            _sprintRepository = new SprintRepository();
            _projectRepository = new ProjectRepository();

            // set new observable collection
            Sprints = new ObservableCollection<SprintModel>();

            // Make commands
            MakeBacklogItemsCommand();
            MakeShowSprintsCommand();
            MakeShowBoardCommand();
            MakeShowSingleProjectCommand();
            MakeCreateSprintCommand();
            CreateDeleteSprintCommand();
            MakeShowSprintCommand();
            MakeShowProjectSettingsCommand();
        }
        public ICommand ShowSprints { get; set; }
        public ICommand ShowBacklogItems { get; set; }
        public ICommand ShowBoardPage { get; set; }
        public ICommand ShowSingleProject { get; set; }
        public ICommand CreateSprintPage { get; set; }
        public ICommand DeleteSprint { get; set; }
        public ICommand ShowSingleSprint { get; set; }
        public ICommand ShowProjectSettings { get; set; }

        public ObservableCollection<SprintModel> Sprints { get; set; }

        public SprintModel ItemSelected { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
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

        public string ProjectTitle
        {
            get => _projectTitle;
            set
            {
                _projectTitle = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Create the command for deleting a sprint
        /// </summary>
        public void CreateDeleteSprintCommand()
        {
            DeleteSprint = new RelayCommand(() =>
            {
                if (_sprintRepository.DeleteSprint(ItemSelected.SprintSlug, ProjectSlug))
                {
                    Messenger.Default.Send(new NotificationMessage("ShowSprintsPage"));
                }
            });
        }

        /// <summary>
        /// Get all the sprints from the api call and set in observable collection
        /// </summary>
        public void GetAllSprintsFromApi()
        {
            // var for return from api call
            var allSprints = _sprintRepository.GetSprints(ProjectSlug);

            if (allSprints == null) return;

            // Add all sprints to observable collection
            foreach (var s in allSprints)
            {
                Sprints.Add(s);
            }
        }

        /// <summary>
        /// Get the project from the api call and set it in the collections
        /// </summary>
        public void GetProjectFromApi()
        {
            var project = _projectRepository.GetOneProject(ProjectSlug);

            if (project == null) return;

            ProjectTitle = project.Title;
        }

        /// <summary>
        /// Make show a sprint command
        /// </summary>
        public void MakeShowSprintCommand()
        {
            ShowSingleSprint = new RelayCommand(() =>
            {
                var sprint = MakeGetSingleSprintModel(ItemSelected.Name, ItemSelected.ProjectId,
                    ItemSelected.StartDate, ItemSelected.EndDate, ItemSelected.SprintSlug, ProjectSlug);
                Messenger.Default.Send(new NotificationMessage(sprint, "ShowSprintPage"));
            });
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
        /// Make a new command for navigating to page for create sprint
        /// </summary>
        private void MakeCreateSprintCommand()
        {
            CreateSprintPage = new RelayCommand(() =>
            {
                Messenger.Default.Send(new NotificationMessage("CreateSprintPage"));
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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}