using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using PrototypeScrumApp.Annotations;
using PrototypeScrumApp.factories;
using PrototypeScrumApp.models;
using PrototypeScrumApp.repositories;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PrototypeScrumApp.viewmodels
{
    public class SingleProjectViewModel : SprintFactory, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ProjectRepository _projectRepository;
        private readonly SprintRepository _sprintRepository;
        private readonly BacklogItemRepository _backlogItemRepository;

        private int _amountOfBacklogItems;
        private int _amountOfStoryPointsBacklog;
        private int _amountOfStoryPointsDone;
        private int _amountOfStoryPointsLeft;
        private int _teamVelocity;
        private string _filePath;
        private string _ideExec;

        [JsonProperty("project_slug")]
        private string _projectSlug;

        [JsonProperty("project_title")]
        private string _projectTitle;


        public SingleProjectViewModel()
        {
            // Set the repositories
            _projectRepository = new ProjectRepository();
            _sprintRepository = new SprintRepository();
            _backlogItemRepository = new BacklogItemRepository();

            // Make observable collections
            SingleProject = new ObservableCollection<SingleProjectModel>();
            Users = new ObservableCollection<UserModel>();
            Sprints = new ObservableCollection<SprintModel>();
            FinishedSprint = new ObservableCollection<SprintModel>();
            BacklogItems = new ObservableCollection<BacklogItemModel>();

            // Make commands
            MakeBacklogItemsCommand();
            MakeShowSprintsCommand();
            MakeShowBoardCommand();
            MakeShowSingleProjectCommand();
            MakeShowSprintCommand();
            MakeShowProjectSettingsCommand();
        }

        public ICommand ShowSprints { get; set; }
        public ICommand ShowBacklogItems { get; set; }
        public ICommand ShowBoardPage { get; set; }
        public ICommand ShowSingleProject { get; set; }
        public ICommand ShowSingleSprint { get; set; }
        public ICommand ShowProjectSettings { get; set; }

        public ObservableCollection<ProjectModel> Project { get; set; }
        public ObservableCollection<SingleProjectModel> SingleProject { get; set; }
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<SprintModel> Sprints { get; set; }
        public ObservableCollection<SprintModel> FinishedSprint { get; set; }
        public ObservableCollection<BacklogItemModel> BacklogItems { get; set; }

        public SprintModel SprintItemSelected { get; set; }

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

        public int AmountOfBacklogItems
        {
            get => _amountOfBacklogItems;
            set
            {
                _amountOfBacklogItems = value;
                OnPropertyChanged();
            }
        }

        public int AmountOfStorypointsBacklog
        {
            get => _amountOfStoryPointsBacklog;
            set
            {
                _amountOfStoryPointsBacklog = value;
                OnPropertyChanged();
            }
        }

        public int AmountOfStorypointsDone
        {
            get => _amountOfStoryPointsDone;
            set
            {
                _amountOfStoryPointsDone = value;
                OnPropertyChanged();
            }
        }

        public int AmountOfStorypointsLeft
        {
            get => _amountOfStoryPointsLeft;
            set
            {
                _amountOfStoryPointsLeft = value;
                OnPropertyChanged();
            }
        }

        public int TeamVelocity
        {
            get => _teamVelocity;
            set
            {
                _teamVelocity = value;
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

            foreach (var user in project.Users)
            {
                Users.Add(user);
            }
        }

        /// <summary>
        /// Get all the sprints from the api call and set in observable collection
        /// </summary>
        public void GetAllSprintsFromApi()
        {
            var allSprints = _sprintRepository.GetSprints(ProjectSlug);

            if (allSprints == null) return;

            foreach (var s in allSprints)
            {
                Sprints.Add(s);
            }
        }

        /// <summary>
        /// Get all the backlog items from the api call and set it into the observable collection
        /// </summary>
        public void GetAllBacklogItemsFromApi()
        {
            var items = _backlogItemRepository.GetAllBacklogItems(ProjectSlug);

            if (items == null) return;

            foreach (var s in Sprints)
            {
                var result = DateTime.Compare(Convert.ToDateTime(s.EndDate), DateTime.Now);
                if (result <= 0)
                {
                    FinishedSprint.Add(s);
                }
            }

            var count = 0;

            foreach (var s in items)
            {
                if (s.StateId == 3)
                {
                    AmountOfStorypointsDone += s.StoryPoints;
                    if (FinishedSprint.Count != 0)
                    {
                        if (Convert.ToInt32(s.SprintId) == FinishedSprint[0].Id)
                        {
                            TeamVelocity += s.StoryPoints;
                        }
                    }
                }

                BacklogItems.Add(s);
                AmountOfStorypointsBacklog += s.StoryPoints;
                AmountOfStorypointsLeft = AmountOfStorypointsBacklog - AmountOfStorypointsDone;
                count++;
            }

            AmountOfBacklogItems = count;
        }

        /// <summary>
        /// Make show a sprint command
        /// </summary>
        public void MakeShowSprintCommand()
        {
            ShowSingleSprint = new RelayCommand(() =>
            {
                var sprint = MakeGetSingleSprintModel(SprintItemSelected.Name, SprintItemSelected.ProjectId,
                    SprintItemSelected.StartDate, SprintItemSelected.EndDate, SprintItemSelected.SprintSlug,
                    ProjectSlug);
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