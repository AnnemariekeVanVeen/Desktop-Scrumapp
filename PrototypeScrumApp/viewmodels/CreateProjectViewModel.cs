using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using PrototypeScrumApp.Annotations;
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
    internal class CreateProjectViewModel : ProjectFactory, INotifyPropertyChanged
    {
        private readonly UserRepository _userRepository;
        private readonly ProjectRepository _projectRepository;
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty("title")]
        private string _title;

        [JsonProperty("description")]
        private string _description;

        [JsonProperty("deadline")]
        private string _deadline;

        private bool _checkBoxChecked;

        public CreateProjectViewModel()
        {
            // Set project repository
            _projectRepository = new ProjectRepository();
            _userRepository = new UserRepository();

            // Set observable collection of users
            UsersCollection = new ObservableCollection<UserModel>();

            // Set list with users (id only)
            UsersList = new List<int>();

            // Make commands
            MakeCreateCommand();

            // Call api endpoints
            GetAllUsersFromApi();
        }

        public ICommand AddProject { get; set; }

        public IList<int> UsersList { get; set; }

        public ObservableCollection<UserModel> UsersCollection { get; set; }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
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

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public string Deadline
        {
            get => _deadline;
            set
            {
                _deadline = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Get all users from api and set in observable collection
        /// </summary>
        public void GetAllUsersFromApi()
        {
            var allUsers = _userRepository.GetUsers<UserModel>();

            if (allUsers == null) return;

            // Add all sprints to observable collection
            foreach (var u in allUsers)
            {
                UsersCollection.Add(u);
            }
        }

        /// <summary>
        /// Make a new command for creating new project
        /// </summary>
        private void MakeCreateCommand()
        {
            AddProject = new RelayCommand(() =>
           {
                // Loop through users collection and add user id to list where checkbox is checked
               foreach (var u in UsersCollection)
               {
                   if (u.CheckBoxChecked)
                   {
                       UsersList.Add(u.Id);
                   }
               }

               if (_projectRepository.CreateProject(MakeCreateAndUpdateProjectModel(Title, Description, Deadline, UsersList)))
               {
                   Messenger.Default.Send(new NotificationMessage("ShowProjectOverview"));
               }
           });
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
