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
using System.Windows;
using System.Windows.Input;

namespace PrototypeScrumApp.viewmodels
{
    internal class LoginViewModel : UserFactory, INotifyPropertyChanged
    {
        private readonly UserRepository _userRepository;
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty("email")]
        private string _email;

        [JsonProperty("password")]
        private string _password;

        public LoginViewModel()
        {
            // Set repositories
            _userRepository = new UserRepository();

            // Set observable collections
            Users = new ObservableCollection<UserModel>();

            // Set commands
            MakeSubmitLoginCommand();

            if (Properties.Settings.Default.db_status != "OK") return;
            MakeSubmitFromListCommand();
            MakeCheckApiCommand();

            var users = _userRepository.RetrieveFromDb();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        public ICommand SubmitLogin { get; set; }
        public ICommand SubmitLoginFromList { get; set; }
        public ICommand CheckApi { get; set; }

        public ObservableCollection<UserModel> Users { get; set; }

        public UserModel ItemSelected { get; set; }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Create submit login command
        /// </summary>
        private void MakeSubmitLoginCommand()
        {
            SubmitLogin = new RelayCommand(() =>
           {
               if (_userRepository.Login(MakeLoginModel(Email, Password)))
               {
                   Messenger.Default.Send(new NotificationMessage("LoginSuccess"));
               }
           });
        }

        /// <summary>
        /// Create submit from list command
        /// </summary>
        private void MakeSubmitFromListCommand()
        {
            SubmitLoginFromList = new RelayCommand(() =>
            {
                if (ItemSelected != null)
                {
                    if (_userRepository.Login(MakeLoginModel(ItemSelected.Email, Password)))
                    {
                        Messenger.Default.Send(new NotificationMessage("LoginSuccess"));
                    }
                }
                else
                {
                    MessageBox.Show("Select a email!");
                }
            });
        }

        /// <summary>
        /// Make check api token command
        /// </summary>
        private void MakeCheckApiCommand()
        {
            CheckApi = new RelayCommand(() => { _userRepository.CheckApiToken(ItemSelected.ApiToken); });
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
