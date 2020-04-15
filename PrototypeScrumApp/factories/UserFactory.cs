using PrototypeScrumApp.models;

namespace PrototypeScrumApp.factories
{
    public class UserFactory
    {
        /// <summary>
        /// Create user model for the login
        /// </summary>
        /// <param name="email"> String email</param>
        /// <param name="password"> String password</param>
        /// <returns> User model for the login functionality</returns>
        public UserModel MakeLoginModel(string email, string password)
        {
            return new UserModel()
            {
                Email = email,
                Password = password
            };
        }

        /// <summary>
        /// Create user model for the register functionality
        /// </summary>
        /// <param name="name"> String name</param>
        /// <param name="email"> String email</param>
        /// <param name="inviteCode"> String invite code</param>
        /// <param name="password"> String password</param>
        /// <param name="passwordConfirmation"> String confirmation password</param>
        /// <returns> User model with data for the registration functionality</returns>
        public UserModel MakeRegisterModel(string name, string email, string inviteCode, string password,
            string passwordConfirmation)
        {
            return new UserModel()
            {
                Name = name,
                Email = email,
                InviteCode = inviteCode,
                Password = password,
                RepeatPassword = passwordConfirmation
            };
        }

        /// <summary>
        /// Create user model for the update user functionality
        /// </summary>
        /// <param name="id"> Int id</param>
        /// <param name="name"> String name</param>
        /// <param name="email"> String email</param>
        /// <param name="password"> String password</param>
        /// <param name="passwordConfirmation"> String confirmation password</param>
        /// <returns> User model for the update functionality</returns>
        public UserModel MakeUpdateUserModel(int id, string name, string email, string password, string passwordConfirmation)
        {
            return new UserModel()
            {
                Id = id,
                Name = name,
                Email = email,
                Password = password,
                RepeatPassword = passwordConfirmation
            };
        }

        /// <summary>
        /// Make user model from the user of the database
        /// </summary>
        /// <param name="email"> String email</param>
        /// <param name="apiToken"> String api token</param>
        /// <param name="id"> Int id</param>
        /// <returns> User model from the user of the database</returns>
        public UserModel MakeUserModelFromDb(string email, string apiToken, int id)
        {
            return new UserModel()
            {
                Email = email,
                ApiToken = apiToken,
                Id = id
            };
        }
    }
}
