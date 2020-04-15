using PrototypeScrumApp.factories;
using PrototypeScrumApp.helpers;
using PrototypeScrumApp.models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;

namespace PrototypeScrumApp.repositories
{
    internal class UserRepository : ApiRepository
    {
        private readonly ResponseHandler _responseHandler;
        private readonly UserFactory _userFactory;

        public UserRepository()
        {
            _responseHandler = new ResponseHandler();
            _userFactory = new UserFactory();
        }

        /// <summary>
        /// Handle create new user api call
        /// </summary>
        /// <param name="user">User model obj</param>
        /// <returns>Bool true if was success. false if there is something wrong</returns>
        public bool Register(UserModel user)
        {
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.RepeatPassword))
            {
                MessageBox.Show("Don't leave the fields empty");
                return false;
            }

            var response = Post<UserModel>("/api/v2/users", user);

            if (!_responseHandler.CheckForErrorResponse(response))
            {
                MessageBox.Show("successfully created a new account");
                return true;
            }

            MessageBox.Show("Register failed try again!");
            return false;
        }

        /// <summary>
        /// Handle authenticate a user api call
        /// </summary>
        /// <param name="user">User model obj</param>
        /// <returns> Boolean true if the call was a success false if there is something wrong</returns>
        public bool Login(UserModel user)
        {
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                MessageBox.Show("Don't leave the fields empty");
                return false;
            }

            try
            {
                var response = Post<UserModel>("/api/v2/auth/create", user);

                var userDb = _responseHandler.SingleObjectResponse<UserModel>(response, "User");

                // Set api token in settings, not saved so that it gets removed after the session is finished
                Properties.Settings.Default.active_api_token = userDb.ApiToken;

                // Disable settings button if database installation failed
                if (Properties.Settings.Default.db_status == "OK")
                {
                    // Add user to the database
                    AddToDb(userDb);
                }

                if (_responseHandler.CheckForErrorResponse(response))
                {
                    return true;
                }

                MessageBox.Show("Login failed try again!");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// Handle update a user api call
        /// </summary>
        /// <param name="user"> Model of the updated user</param>
        /// <returns> Boolean true if the call was a success false if there is something wrong</returns>
        public bool Update(UserModel user)
        {
            if (string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.RepeatPassword) || (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Name)))
            {
                MessageBox.Show("Don't leave fields empty");
                return false;
            }

            var response = Put<UserModel>($"/api/v2/users/{user.Id}", user);

            if (_responseHandler.CheckForErrorResponse(response))
            {
                MessageBox.Show("Details were updated successfully");
                return true;
            }

            MessageBox.Show("Update details failed try again!");
            return false;
        }

        /// <summary>
        /// Handle check if the api token is valid/exists api call
        /// </summary>
        /// <param name="apiToken">Api token retrieved from the view (item selected)</param>
        /// <returns>String if the call was a success or failed.</returns>
        public string CheckApiToken(string apiToken)
        {
            // Setup client
            Client = new HttpClient { BaseAddress = new Uri(Properties.Settings.Default.Api_url) };
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(Properties.Settings.Default.Application_json));

            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiToken);

            var response = Client.GetStringAsync("/api/v2/auth/show").Result;

            if (_responseHandler.CheckForErrorResponse(response))
            {
                return response;
            }

            MessageBox.Show("Your token is invalid, this account will be deleted from the list.");
            DeleteFromDb(apiToken);
            return response;
        }

        /// <summary>
        /// Handle get all users api call 
        /// </summary>
        /// <returns>IList of users. null if something went wrong</returns>
        public IList<TUserModel> GetUsers<TUserModel>()
        {
            var response = Get("/api/v2/users");

            return _responseHandler.CheckForErrorResponse(response) ? _responseHandler.ListOfObjectsResponse<TUserModel>(response, "Users") : null;
        }

        /// <summary>
        /// Function to add user to db
        /// </summary>
        /// <param name="userDb">User model userDb</param>
        private static void AddToDb(UserModel userDb)
        {
            // Get connection string out of settings
            var cnString = Properties.Settings.Default.connectionString;
            // Make new SQL Connection
            var connection = new SqlConnection(cnString);
            // Open connection
            connection.Open();

            // SQL Query
            var sqlInsert = "IF NOT EXISTS(SELECT * FROM users WHERE api_token='" + userDb.ApiToken + "') BEGIN INSERT INTO users (email, api_token) VALUES ('" + userDb.Email + "', '" + userDb.ApiToken + "') END";
            // Set new SQL Command
            var sqlCommand = new SqlCommand(sqlInsert, connection);
            // Execute SQL Command
            sqlCommand.ExecuteNonQuery();
            // Close connection            
            connection.Close();
        }

        /// <summary>
        /// Retrieve the accounts that are saved in the database.
        /// </summary>
        /// <returns>IList containing all the users in the database.</returns>
        public IList<UserModel> RetrieveFromDb()
        {
            // Declare IList
            IList<UserModel> users = new List<UserModel>();
            // Set new SQL connection
            var connection = new SqlConnection(Properties.Settings.Default.connectionString);
            // Set SQL command to be executed
            var sqlCommand = new SqlCommand("SELECT * FROM users", connection);
            try
            {
                // Open connection
                connection.Open();
                // Set new dataReader and execute reader with SQL command
                var dataReader = sqlCommand.ExecuteReader();
                // While reading the data gets added to the IList as a user model
                while (dataReader.Read())
                {
                    var apiToken = (string)dataReader["api_token"];
                    var email = (string)dataReader["email"];
                    var id = (int)dataReader["id"];
                    users.Add(_userFactory.MakeUserModelFromDb(email, apiToken, id));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                connection.Close();
            }
            return users;
        }

        /// <summary>
        /// Function that deletes a specific user from the database
        /// </summary>
        /// <param name="apiToken">Api token from account that has to be removed due to invalid token.</param>
        public void DeleteFromDb(string apiToken)
        {
            // Get connection string out of settings
            var cnString = Properties.Settings.Default.connectionString;
            // Make new SQL Connection
            var connection = new SqlConnection(cnString);
            // Open connection
            connection.Open();

            // SQL Query
            var sqlInsert = "DELETE FROM users WHERE api_token='" + apiToken + "'";
            // Set new SQL Command
            var sqlCommand = new SqlCommand(sqlInsert, connection);
            // Execute SQL Command
            sqlCommand.ExecuteNonQuery();
            // Close connection            
            connection.Close();
        }
    }
}
