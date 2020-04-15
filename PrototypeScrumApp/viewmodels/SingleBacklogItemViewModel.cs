using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using PrototypeScrumApp.Annotations;
using PrototypeScrumApp.factories;
using PrototypeScrumApp.models;
using PrototypeScrumApp.repositories;

namespace PrototypeScrumApp.viewmodels
{
    public class SingleBacklogItemViewModel : BacklogItemFactory, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly BacklogItemRepository _backlogItemRepository;
        private readonly ProjectRepository _projectRepository;
        private BacklogTypeModel _backlogTypeModel;

        [JsonProperty("project_slug")]
        private string _projectSlug;

        [JsonProperty("id")]
        private int _backlogItemId;

        [JsonProperty("project_title")]
        private string _projectTitle;

        /// <summary>
        /// Constructor of the ViewModel
        /// </summary>
        public SingleBacklogItemViewModel()
        {
            // Set repository
            _backlogItemRepository = new BacklogItemRepository();
            _projectRepository = new ProjectRepository();

            // Set new observable collection
            BacklogItems = new ObservableCollection<BacklogItemModel>();
            BacklogTypes = new ObservableCollection<BacklogTypeModel>();

            // Set commands
            CreateUpdateCommand();
            MakeBacklogItemsCommand();
            MakeUpdateBacklogItemsCommand();
            MakeShowSprintsCommand();
            MakeShowBoardCommand();
            MakeShowSingleProjectCommand();
            MakeCreateBacklogItemCommand();
            MakeShowProjectSettingsCommand();
        }

        public ObservableCollection<BacklogItemModel> BacklogItems { get; set; }
        public ObservableCollection<BacklogTypeModel> BacklogTypes { get; set; }

        public BacklogItemModel ItemSelected { get; set; }

        public ICommand ShowSprints { get; set; }
        public ICommand ShowBacklogItems { get; set; }
        public ICommand ShowBoardPage { get; set; }
        public ICommand ShowSingleProject { get; set; }
        public ICommand CreateBacklogItemPage { get; set; }
        public ICommand UpdateBacklogItem { get; set; }
        public ICommand ShowProjectSettings { get; set; }

        public int BacklogItemId
        {
            get => _backlogItemId;
            set
            {
                _backlogItemId = value;
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
        /// Get the backlog item from the api call and set it in the collections
        /// </summary>
        public void GetBacklogItemFromApi()
        {
            var backlogItem = _backlogItemRepository.GetOneBacklogItem(ProjectSlug, BacklogItemId);

            if (backlogItem == null) return;

            BacklogItems.Add(backlogItem);
        }

        /// <summary>
        /// Get all the backlog items from the api call and set it into the observable collection
        /// </summary>
        public void GetAllBacklogItemsFromApi()
        {
            var items = _backlogItemRepository.GetAllBacklogItems(ProjectSlug);

            if (items == null) return;

            foreach (var s in items)
            {
                BacklogItems.Add(s);
            }
        }

        /// <summary>
        /// Create a command to update a backlog item
        /// </summary>
        public void CreateUpdateCommand()
        {
            UpdateBacklogItem = new RelayCommand(() =>
            {
                if (_backlogItemRepository.UpdateBacklogItem(ProjectSlug, Convert.ToInt32(BacklogItems[0].BacklogItemId),
                    MakeUpdateBacklogItemModel(BacklogItems[0].BacklogItemId, BacklogItems[0].UserRole, BacklogItems[0].Action, BacklogItems[0].StoryPoints,
                        BacklogType.Id, Convert.ToInt32(BacklogItems[0].StateId), Convert.ToInt32(BacklogItems[0].SprintId))))
                {
                    MessageBox.Show("Updated successfully");
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
        /// Make a new command for navigating to backlogItems view
        /// </summary>
        private void MakeUpdateBacklogItemsCommand()
        {
            ShowBacklogItems = new RelayCommand(() =>
            {
                Messenger.Default.Send(new NotificationMessage("ShowUpdateBacklogItemsPage"));
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
        /// Make a new command for navigating to page for create backlog item
        /// </summary>
        private void MakeCreateBacklogItemCommand()
        {
            CreateBacklogItemPage = new RelayCommand(() =>
            {
                Messenger.Default.Send(new NotificationMessage("CreateBacklogItemPage"));
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
