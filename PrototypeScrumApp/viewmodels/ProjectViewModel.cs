using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using PrototypeScrumApp.factories;
using PrototypeScrumApp.models;
using PrototypeScrumApp.repositories;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PrototypeScrumApp.viewmodels
{
    public class ProjectViewModel : ProjectFactory, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ProjectRepository _projectRepository;

        [JsonProperty("description")]
        private string _description;

        public ProjectViewModel()
        {
            // Set project repository
            _projectRepository = new ProjectRepository();

            // Set observable collection
            Projects = new ObservableCollection<ProjectModel>();

            // Set commands
            MakeDeleteCommand();
            MakeShowProjectCommand();
            MakeUpdateProjectCommand();

            // Call api endpoints
            GetAllProjectsApiCall();
        }

        public ICommand DeleteProject { get; set; }
        public ICommand ShowProject { get; set; }
        public ICommand UpdateProject { get; set; }

        public ObservableCollection<ProjectModel> Projects { get; set; }

        public ProjectModel ItemSelected { get; set; }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Api call to get all the projects in the observable collection
        /// </summary>
        private void GetAllProjectsApiCall()
        {
            var allProjects = _projectRepository.GetAllProjects();

            if (allProjects == null) return;

            foreach (var s in allProjects)
            {
                Projects.Add(s);
            }
        }

        /// <summary>
        /// Make a new command for deleting a project
        /// </summary>
        private void MakeDeleteCommand()
        {
            DeleteProject = new RelayCommand(() =>
            {
                if (_projectRepository.DeleteProject(MakeSelectedProjectModel(ItemSelected.Title,
                    ItemSelected.Description, ItemSelected.Deadline, ItemSelected.ProjectSlug)))
                {
                    Messenger.Default.Send(new NotificationMessage("RefreshProjectPage"));
                }
            });
        }

        /// <summary>
        /// Make a new command for showing a project
        /// </summary>
        public void MakeShowProjectCommand()
        {
            ShowProject = new RelayCommand(() =>
           {
               var project = MakeSelectedProjectModel(ItemSelected.Title, ItemSelected.Description, ItemSelected.Deadline,
                   ItemSelected.ProjectSlug);
               Messenger.Default.Send(new NotificationMessage(project, "ShowProjectPage"));
           });
        }

        /// <summary>
        /// Making a new command for showing the update page of a project
        /// </summary>
        public void MakeUpdateProjectCommand()
        {
            UpdateProject = new RelayCommand(() =>
            {
                var project = MakeSelectedProjectModel(ItemSelected.Title, ItemSelected.Description, ItemSelected.Deadline,
                    ItemSelected.ProjectSlug);
                Messenger.Default.Send(new NotificationMessage(project, "ShowUpdateProjectPage"));
            });
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
