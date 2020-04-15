using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using PrototypeScrumApp.Annotations;
using PrototypeScrumApp.factories;
using PrototypeScrumApp.repositories;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PrototypeScrumApp.viewmodels
{
    public class RegisterViewModel : UserFactory, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly UserRepository _userRepository;

        [JsonProperty("name")]
        private string _name;

        [JsonProperty("email")]
        private string _email;

        [JsonProperty("invite_code")]
        private string _inviteCode;

        [JsonProperty("password")]
        private string _password;

        [JsonProperty("password_confirmation")]
        private string _repeatPassword;

        public RegisterViewModel()
        {
            // Set repository
            _userRepository = new UserRepository();

            // Create a register command
            CreateRegisterCommand();
        }

        public ICommand SubmitRegister { get; set; }

        public string Name
        {
            get => _name;
            set
            {

                _name = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string InviteCode
        {
            get => _inviteCode;
            set
            {
                _inviteCode = value;
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
        /// Create a command to handle submit register 
        /// </summary>
        public void CreateRegisterCommand()
        {
            SubmitRegister = new RelayCommand(() =>
            {
                if (_userRepository.Register(MakeRegisterModel(Name, Email, InviteCode, Password, RepeatPassword)))
                {
                    Messenger.Default.Send(new NotificationMessage("RegisterSuccess"));
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
