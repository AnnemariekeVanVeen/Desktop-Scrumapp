using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using PrototypeScrumApp.Annotations;
using PrototypeScrumApp.factories;
using PrototypeScrumApp.helpers;
using PrototypeScrumApp.models;
using PrototypeScrumApp.repositories;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RelayCommand = GalaSoft.MvvmLight.Command.RelayCommand;

namespace PrototypeScrumApp.viewmodels
{
    internal class UserViewModel : UserFactory, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly UserRepository _userRepository;
        private readonly ResponseHandler _responseHandler;

        [JsonProperty("name")]
        private string _name;

        [JsonProperty("email")]
        private string _email;

        [JsonProperty("password")]
        private string _password;

        [JsonProperty("password_confirmation")]
        private string _repeatPassword;

        public UserViewModel()
        {
            // Set repository
            _userRepository = new UserRepository();

            // Set response handler
            _responseHandler = new ResponseHandler();

            // Set new observable collection
            UserObject = new ObservableCollection<UserModel>();

            // Set commands
            MakeUpdateUserCommand();

            // Get user
            ShowUserFromToken();
        }

        public ICommand UpdateUserCommand { get; set; }

        public ObservableCollection<UserModel> UserObject { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
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

        public string RepeatPassword
        {
            get => _repeatPassword;
            set
            {
                _repeatPassword = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Make update user command
        /// </summary>
        public void MakeUpdateUserCommand()
        {
            UpdateUserCommand = new RelayCommand(() =>
            {
                if (_userRepository.Update(MakeUpdateUserModel(UserObject[0].Id, UserObject[0].Name, UserObject[0].Email, Password, RepeatPassword)))
                {
                    Messenger.Default.Send(new NotificationMessage("ShowUserPage"));
                }
            });
        }

        /// <summary>
        /// Function to show one user.
        /// CheckApiToken already existed in our software and is the same as List One User, so that is why we use this api call.
        /// </summary>
        public void ShowUserFromToken()
        {
            var response = _userRepository.CheckApiToken(Properties.Settings.Default.active_api_token);

            if (!_responseHandler.CheckForErrorResponse(response)) return;

            var userObject = _responseHandler.SingleObjectResponse<UserModel>(response, "User");
            UserObject.Add(userObject);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
