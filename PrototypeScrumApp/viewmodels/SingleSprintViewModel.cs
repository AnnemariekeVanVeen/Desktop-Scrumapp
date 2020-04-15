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
    internal class SingleSprintViewModel : SprintFactory, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly SprintRepository _sprintRepository;
        private readonly BacklogItemRepository _backlogItemRepository;

        [JsonProperty("project_slug")]
        private string _projectSlug;

        [JsonProperty("sprint_slug")]
        private string _sprintSlug;

        [JsonProperty("name")]
        private string _name;

        [JsonProperty("in_sprint")]
        private readonly bool _inSprint = true;

        public SingleSprintViewModel()
        {
            // Set the repositories
            _sprintRepository = new SprintRepository();
            _backlogItemRepository = new BacklogItemRepository();

            // set observable collections
            Sprint = new ObservableCollection<SprintModel>();
            BacklogItems = new ObservableCollection<BacklogItemModel>();
            SprintBacklogItems = new ObservableCollection<BacklogItemModel>();

            // make commands
            MakeBacklogItemsCommand();
            MakeShowSprintsCommand();
            MakeShowBoardCommand();
            MakeShowSingleProjectCommand();
            MakeUpdateSprintCommand();
            MakeShowProjectSettingsCommand();
            UpdateBacklogItemInSprint();
        }

        public ObservableCollection<SprintModel> Sprint { get; set; }
        public ObservableCollection<BacklogItemModel> BacklogItems { get; set; }
        public ObservableCollection<BacklogItemModel> SprintBacklogItems { get; set; }

        public BacklogItemModel BacklogItemSelected { get; set; }
        public BacklogItemModel SprintBacklogItemSelected { get; set; }

        public ICommand UpdateSprint { get; set; }
        public ICommand ShowSprints { get; set; }
        public ICommand ShowBacklogItems { get; set; }
        public ICommand ShowBoardPage { get; set; }
        public ICommand ShowSingleProject { get; set; }
        public ICommand ShowSingleSprint { get; set; }
        public ICommand ShowProjectSettings { get; set; }
        public ICommand UpdateBacklogInItem { get; set; }

        public string ProjectSlug
        {
            get => _projectSlug;
            set
            {
                _projectSlug = value;
                OnPropertyChanged();
            }
        }

        public string SprintSlug
        {
            get => _sprintSlug;
            set
            {
                _sprintSlug = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Get the sprint from the api
        /// </summary>
        public void GetSprintFromApi()
        {
            var sprintModel = _sprintRepository.GetOneSprint(ProjectSlug, SprintSlug);

            if (sprintModel != null)
            {
                Sprint.Add(sprintModel);
            }
        }

        /// <summary>
        /// Get the sprint backlog items from api
        /// </summary>
        public void GetSprintBacklogItemsFromApi()
        {
            var response = _sprintRepository.GetAllBacklogItemsOfOneSprint(ProjectSlug, SprintSlug);

            if (response == null) return;

            foreach (var s in response)
            {
                SprintBacklogItems.Add(s);
            }
        }


        /// <summary>
        /// Get the backlog items from api that are not included in a print
        /// </summary>
        public void GetAllBacklogItemsFromApi()
        {
            var items = _backlogItemRepository.GetAllBacklogItems(ProjectSlug);

            // Checks if response is okay
            if (items == null) return;

            // Add all backlogItems to observable collection
            foreach (var s in items)
            {
                if (s.SprintId != Sprint[0].Id)
                {
                    BacklogItems.Add(s);
                }
            }
        }

        /// <summary>
        /// Update backlog item to sprint
        /// </summary>
        public void UpdateBacklogItemInSprint()
        {
            UpdateBacklogInItem = new RelayCommand(() =>
            {
                var sprint = MakeGetSingleSprintModel(Sprint[0].Name, Sprint[0].ProjectId,
                    Sprint[0].StartDate, Sprint[0].EndDate, Sprint[0].SprintSlug, ProjectSlug);
                if (BacklogItemSelected != null)
                {
                    var id = BacklogItemSelected.ItemId = BacklogItemSelected.BacklogItemId;
                    BacklogItemSelected.InSprint = true;
                    var backlogItem = BacklogItemFactory.AddBacklogItem(id, _inSprint);
                    _sprintRepository.UpdateBacklogItem(ProjectSlug, SprintSlug, backlogItem);
                    Messenger.Default.Send(new NotificationMessage(sprint, "ShowSprintPage"));
                }
                else
                {
                    var id = SprintBacklogItemSelected.ItemId = SprintBacklogItemSelected.BacklogItemId;
                    SprintBacklogItemSelected.InSprint = false;
                    var backlogItem = BacklogItemFactory.AddBacklogItem(id, SprintBacklogItemSelected.InSprint);
                    _sprintRepository.UpdateBacklogItem(ProjectSlug, SprintSlug, backlogItem);
                    Messenger.Default.Send(new NotificationMessage(sprint, "ShowSprintPage"));
                }
            });
        }


        /// <summary>
        /// Make the update command
        /// </summary>
        private void MakeUpdateSprintCommand()
        {
            UpdateSprint = new RelayCommand(() =>
            {
                var sprint = MakeUpdateSprintModel(Sprint[0].Name, Sprint[0].StartDate,
                    Sprint[0].EndDate, ProjectSlug, SprintSlug);
                if (_sprintRepository.UpdateSprint(sprint))
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
