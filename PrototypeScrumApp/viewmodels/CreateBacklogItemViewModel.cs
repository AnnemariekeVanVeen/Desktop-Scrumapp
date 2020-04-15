using GalaSoft.MvvmLight.CommandWpf;
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
    internal class CreateBacklogItemViewModel : BacklogItemFactory, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly BacklogItemRepository _backlogItemRepository;
        private BacklogTypeModel _backlogTypeModel;

        [JsonProperty("action")]
        private string _action;

        [JsonProperty("user_role")]
        private string _userRole;

        [JsonProperty("story_points")]
        private int _storyPoints;

        [JsonProperty("backlog_type_id")]
        private int _backlogTypeId;

        [JsonProperty("project_slug")]
        private string _projectSlug;


        public CreateBacklogItemViewModel()
        {
            // Set the repository
            _backlogItemRepository = new BacklogItemRepository();

            // Set observable collections
            BacklogTypes = new ObservableCollection<BacklogTypeModel>();

            // Set commands
            MakeBacklogItemsCommand();
            MakeShowSprintsCommand();
            MakeShowBoardCommand();
            MakeShowSingleProjectCommand();
            CreateNewBacklogItemCommand();
            MakeShowProjectSettingsCommand();

            // Call api endpoints
            GetBacklogTypesFromApi();
        }

        public ObservableCollection<BacklogTypeModel> BacklogTypes { get; set; }

        public ICommand ShowSprints { get; set; }
        public ICommand ShowBacklogItems { get; set; }
        public ICommand ShowBoardPage { get; set; }
        public ICommand ShowSingleProject { get; set; }
        public ICommand AddBacklogItem { get; set; }
        public ICommand ShowProjectSettings { get; set; }

        public string Action
        {
            get => _action;
            set
            {
                _action = value;
                OnPropertyChanged();
            }
        }

        public string UserRole
        {
            get => _userRole;
            set
            {
                _userRole = value;
                OnPropertyChanged();
            }
        }

        public int StoryPoints
        {
            get => _storyPoints;
            set
            {
                _storyPoints = value;
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

        public BacklogTypeModel BacklogType
        {
            get => _backlogTypeModel;
            set
            {
                _backlogTypeModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Create a command to add a new backlog item
        /// </summary>
        public void CreateNewBacklogItemCommand()
        {
            AddBacklogItem = new RelayCommand(() =>
           {
               if (_backlogItemRepository.AddBacklogItem(MakeNewBacklogItemModel(UserRole, Action, StoryPoints, BacklogType.Id), ProjectSlug))
               {
                   Messenger.Default.Send(new NotificationMessage("ShowBacklogItemsPage"));
               }
           });
        }

        /// <summary>
        /// Get all the backlog types from api
        /// </summary>
        public void GetBacklogTypesFromApi()
        {
            var items = _backlogItemRepository.GetBacklogItemTypes();

            if (items == null) return;

            foreach (var s in items)
            {
                BacklogTypes.Add(s);
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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
