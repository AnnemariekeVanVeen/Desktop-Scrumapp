using GalaSoft.MvvmLight.CommandWpf;
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
using System.Windows;
using System.Windows.Input;

namespace PrototypeScrumApp.viewmodels
{
    internal class BacklogItemViewModel : BacklogItemFactory, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly BacklogItemRepository _backlogItemRepository;
        private readonly ProjectRepository _projectRepository;

        [JsonProperty("project_slug")]
        private string _projectSlug;

        [JsonProperty("project_title")]
        private string _projectTitle;

        public BacklogItemViewModel()
        {
            // Set repository
            _backlogItemRepository = new BacklogItemRepository();
            _projectRepository = new ProjectRepository();

            // Set new observable collection
            BacklogItems = new ObservableCollection<BacklogItemModel>();
            BacklogTypes = new ObservableCollection<BacklogTypeModel>();

            // Set commands
            CreateDeleteCommand();
            MakeShowBacklogItemCommand();
            MakeBacklogItemsCommand();
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
        public ICommand DeleteBacklogItem { get; set; }
        public ICommand CreateBacklogItemPage { get; set; }
        public ICommand UpdateBacklogItem { get; set; }
        public ICommand ShowProjectSettings { get; set; }

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
        /// Make show a backlog item command
        /// </summary>
        public void MakeShowBacklogItemCommand()
        {
            UpdateBacklogItem = new RelayCommand(() =>
            {
                var backlogItem = MakeUpdateBacklogItemModel(ItemSelected.BacklogItemId, ItemSelected.UserRole, ItemSelected.Action, ItemSelected.StoryPoints,
                    Convert.ToInt32(ItemSelected.BacklogTypeId), Convert.ToInt32(ItemSelected.StateId), Convert.ToInt32(ItemSelected.SprintId));
                backlogItem.ProjectSlug = ProjectSlug;
                Messenger.Default.Send(new NotificationMessage(backlogItem, "ShowUpdateBacklogItemsPage"));
            });
        }

        /// <summary>
        /// Create a command to delete a backlog item
        /// </summary>
        public void CreateDeleteCommand()
        {
            // Set delete command
            DeleteBacklogItem = new RelayCommand(() =>
            {
                if (_backlogItemRepository.Delete(ProjectSlug, ItemSelected.BacklogItemId.ToString()))
                {
                    Messenger.Default.Send(new NotificationMessage("ShowBacklogItemsPage"));
                }
            });
        }

        /// <summary>
        /// Get all the backlog items and set it into the observable collection
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
        /// Get the project from the api call and set it in the collections
        /// </summary>
        public void GetProjectFromApi()
        {
            var project = _projectRepository.GetOneProject(ProjectSlug);

            if (project != null)
            {
                ProjectTitle = project.Title;
            }
        }

        /// <summary>
        /// Get all the backlog types from the api
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
