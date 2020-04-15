using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using PrototypeScrumApp.Annotations;
using PrototypeScrumApp.factories;
using PrototypeScrumApp.repositories;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PrototypeScrumApp.viewmodels
{
    internal class CreateSprintViewModel : SprintFactory, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly SprintRepository _sprintRepository;

        [JsonProperty("name")]
        private string _name;

        [JsonProperty("project_id")]
        private string _projectId;

        [JsonProperty("end_date")]
        private string _endDate;

        [JsonProperty("start_date")]
        private string _startDate;

        [JsonProperty("project_slug")]
        private string _projectSlug;

        public CreateSprintViewModel()
        {
            // Set sprint repository
            _sprintRepository = new SprintRepository();

            // Make commands
            MakeBacklogItemsCommand();
            MakeShowSprintsCommand();
            MakeShowBoardCommand();
            MakeShowSingleProjectCommand();
            CreateAddNewSprintCommand();
            MakeShowProjectSettingsCommand();
        }

        public ICommand AddSprint { get; set; }
        public ICommand ShowSprints { get; set; }
        public ICommand ShowBacklogItems { get; set; }
        public ICommand ShowBoardPage { get; set; }
        public ICommand ShowSingleProject { get; set; }
        public ICommand ShowProjectSettings { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string ProjectId
        {
            get => _projectId;
            set
            {
                _projectId = value;
                OnPropertyChanged();
            }
        }

        public string StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }

        public string EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
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

        /// <summary>
        /// Create command to add a new sprint
        /// </summary>
        public void CreateAddNewSprintCommand()
        {
            AddSprint = new RelayCommand(() =>
           {
               if (_sprintRepository.CreateSprint(
                   MakeCreateSprintModel(Name, ProjectId, StartDate, EndDate, ProjectSlug)))
               {
                   Messenger.Default.Send(new NotificationMessage("ShowSprintsPage"));
               }
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
