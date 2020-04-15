using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using PrototypeScrumApp.factories;
using PrototypeScrumApp.models;
using PrototypeScrumApp.repositories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PrototypeScrumApp.viewmodels
{
    public class UpdateProjectViewModel : ProjectFactory, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ProjectRepository _projectRepository;
        private readonly UserRepository _userRepository;

        [JsonProperty("deadline")]
        private string _deadline;

        [JsonProperty("slug")]
        private string _projectSlug;

        public UpdateProjectViewModel()
        {
            // Set repositories
            _projectRepository = new ProjectRepository();
            _userRepository = new UserRepository();

            // Make observable collections
            SingleProject = new ObservableCollection<SingleProjectModel>();
            UsersInProject = new ObservableCollection<UserModel>();
            UsersCollection = new ObservableCollection<UserModel>();
            Projects = new ObservableCollection<ProjectModel>();

            // Set list with users (id only)
            UsersList = new List<int>();

            // Set commands
            MakeUpdateProjectCommand();
        }

        public IList<int> UsersList { get; set; }

        public ICommand UpdateProject { get; set; }

        public ObservableCollection<ProjectModel> Projects { get; set; }
        public ObservableCollection<SingleProjectModel> SingleProject { get; set; }
        public ObservableCollection<UserModel> UsersInProject { get; set; }
        public ObservableCollection<UserModel> UsersCollection { get; set; }


        private bool _checkBoxChecked;

        public string Deadline
        {
            get => _deadline;
            set
            {
                _deadline = value;
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

        public bool CheckBoxChecked
        {
            get => _checkBoxChecked;
            set
            {
                _checkBoxChecked = value;
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

            foreach (var user in project.Users)
            {
                UsersInProject.Add(user);
            }
        }

        /// <summary>
        /// Get all users from api and set in observable collection
        /// </summary>
        public void GetAllUsersFromApi()
        {
            var allUsers = _userRepository.GetUsers<UserModel>();

            if (allUsers == null) return;

            foreach (var u in allUsers)
            {
                UsersCollection.Add(u);
            }
        }

        /// <summary>
        /// Making a new command for updating a project
        /// </summary>
        public void MakeUpdateProjectCommand()
        {
            UpdateProject = new RelayCommand(() =>
            {
                // Loop through users collection and add user id to list where checkbox is checked
                foreach (var u in UsersCollection)
                {
                    if (u.CheckBoxChecked)
                    {
                        UsersList.Add(u.Id);
                    }
                }

                RemoveFromCollection();

                if (_projectRepository.UpdateProject(ProjectSlug,
                    MakeCreateAndUpdateProjectModel(SingleProject[0].Title, SingleProject[0].Description, Deadline, UsersList)))
                {
                    Messenger.Default.Send(new NotificationMessage("ShowProjectOverview"));
                }
            });
        }

        /// <summary>
        /// Function which compares two observable collections
        /// Sets the checkbox to true if the user exists in the project
        /// In the view it will be visible which users are already a team member
        /// </summary>
        public void CompareCollections()
        {
            foreach (var userInProject in UsersInProject)
            {
                foreach (var u in UsersCollection)
                {
                    if (u.Id == userInProject.Id)
                    {
                        u.CheckBoxChecked = true;
                    }
                }
            }
        }

        /// <summary>
        /// Function to remove a user id from the collection when the checkbox is returned false
        /// </summary>
        public void RemoveFromCollection()
        {
            foreach (var dummy in UsersInProject)
            {
                foreach (var u in UsersCollection)
                {
                    if (u.CheckBoxChecked == false)
                    {
                        UsersList.Remove(u.Id);
                    }
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
