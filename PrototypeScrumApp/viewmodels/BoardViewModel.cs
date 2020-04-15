using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using PrototypeScrumApp.Annotations;
using PrototypeScrumApp.helpers;
using PrototypeScrumApp.models;
using PrototypeScrumApp.repositories;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PrototypeScrumApp.viewmodels
{
    internal class BoardViewModel : INotifyPropertyChanged
    {
        private SprintModel _sprintItemSelected;
        private readonly ResponseHandler _responseHandler;
        private readonly SprintRepository _sprintRepository;
        private readonly BacklogItemRepository _backlogItemRepository;
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty("project_slug")]
        private string _projectSlug;

        [JsonProperty("project_title")]
        private string _projectTitle;

        public BoardViewModel()
        {
            // Set the repositories
            _sprintRepository = new SprintRepository();
            _backlogItemRepository = new BacklogItemRepository();

            // Make observable collections
            Sprints = new ObservableCollection<SprintModel>();
            BacklogItems = new ObservableCollection<BacklogItemModel>();
            BacklogItemsDone = new ObservableCollection<BacklogItemModel>();
            BacklogItemsDoing = new ObservableCollection<BacklogItemModel>();

            // Set the response handler
            _responseHandler = new ResponseHandler();

            // Make commands
            UpdateStateCommand();
            MakeShowBoardCommand();
            MakeShowSprintsCommand();
            MakeBacklogItemsCommand();
            MakeShowSingleProjectCommand();
            MakeShowProjectSettingsCommand();
        }

        public ICommand UpdateState { get; set; }
        public ICommand ShowSprints { get; set; }
        public ICommand ShowBoardPage { get; set; }
        public ICommand ShowBacklogItems { get; set; }
        public ICommand ShowSingleProject { get; set; }
        public ICommand ShowProjectSettings { get; set; }

        public ObservableCollection<SprintModel> Sprints { get; set; }
        public ObservableCollection<BacklogItemModel> BacklogItems { get; set; }
        public ObservableCollection<BacklogItemModel> BacklogItemsDone { get; set; }
        public ObservableCollection<BacklogItemModel> BacklogItemsDoing { get; set; }

        public BacklogItemModel ItemSelected { get; set; }
        public BacklogItemModel ItemSelectedDone { get; set; }
        public BacklogItemModel ItemSelectedDoing { get; set; }

        public SprintModel SprintItemSelected
        {
            get => _sprintItemSelected;
            set
            {
                _sprintItemSelected = value;
                BacklogItems.Clear();
                BacklogItemsDoing.Clear();
                BacklogItemsDone.Clear();
                GetAllBacklogItemsFromApi();
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
            if (SprintItemSelected.SprintSlug == null) return;

            var response = _backlogItemRepository.GetAllBacklogItemsForBoard(ProjectSlug, SprintItemSelected.SprintSlug);

            if (!_responseHandler.CheckForErrorResponse(response)) return;

            var items = _responseHandler.ListOfObjects<BacklogItemModel>(response, "Kanban", "backlog");

            foreach (var s in items)
            {
                BacklogItems.Add(s);
            }

            var itemsInProgress = _responseHandler.ListOfObjects<BacklogItemModel>(response, "Kanban", "in_progress");

            // Add all backlogItems with stateId=2 observable collection
            foreach (var s in itemsInProgress)
            {
                BacklogItemsDoing.Add(s);
            }

            var itemsDone = _responseHandler.ListOfObjects<BacklogItemModel>(response, "Kanban", "done");

            // Add all backlogItems with stateId=3 observable collection
            foreach (var s in itemsDone)
            {
                BacklogItemsDone.Add(s);
            }
        }

        /// <summary>
        /// Make a new command for updating the state of the backlogItems
        /// </summary>
        private void MakeShowSprintsCommand()
        {
            ShowSprints = new RelayCommand(() =>
            {
                Messenger.Default.Send(new NotificationMessage("ShowSprintsPage"));
            });
        }

        /// <summary>
        /// Make a new command for updating the state on the scrumBoard
        /// </summary>
        private void UpdateStateCommand()
        {
            UpdateState = new RelayCommand(() =>
            {
                _backlogItemRepository.Update(ProjectSlug, SprintItemSelected.SprintSlug,
                    new StateModel(ItemSelected.BacklogItemId, ItemSelected.StateId));
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
