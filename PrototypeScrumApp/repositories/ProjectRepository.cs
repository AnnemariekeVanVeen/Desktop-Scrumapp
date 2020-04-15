using PrototypeScrumApp.helpers;
using PrototypeScrumApp.models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace PrototypeScrumApp.repositories
{
    internal class ProjectRepository : ApiRepository
    {
        private readonly ResponseHandler _responseHandler;

        public ProjectRepository()
        {
            _responseHandler = new ResponseHandler();
        }

        /// <summary>
        /// Handle create project api call
        /// </summary>
        /// <param name="project"> Project model of the new project</param>
        /// <returns>Boolean true if the call was a success false when something went wrong</returns>
        public bool CreateProject(ProjectModel project)
        {
            if (string.IsNullOrEmpty(project.Title) || string.IsNullOrEmpty(project.Deadline))
            {
                MessageBox.Show("Don't leave the fields empty");
                return false;
            }

            var response = Post<ProjectModel>("/api/v2/projects", project);
            if (_responseHandler.CheckForErrorResponseProject(response))
            {
                return true;
            }

            MessageBox.Show("Error project name already exists");
            return false;
        }

        /// <summary>
        /// Handle get all projects from api call
        /// </summary>
        /// <returns> IEnumerable with all the projects from the api. returns null if something happened</returns>
        public IEnumerable<ProjectModel> GetAllProjects()
        {
            var projectsResponse = Get("/api/v2/projects");

            if (!_responseHandler.CheckForErrorResponse(projectsResponse)) return null;

            var ownProjects = _responseHandler.ListOfObjectsResponse<ProjectModel>(projectsResponse, "OwnProjects");
            var projects = _responseHandler.ListOfObjectsResponse<ProjectModel>(projectsResponse, "Projects");

            return projects.Concat(ownProjects);
        }

        /// <summary>
        /// Handle  update project api call
        /// </summary>
        /// <param name="projectSlug"> Slug of the project</param>
        /// <param name="project"> Model of the updated project</param>
        /// <returns>Boolean true if the call was a success false when something went wrong</returns>
        public bool UpdateProject(string projectSlug, ProjectModel project)
        {
            if (string.IsNullOrEmpty(project.Title) || string.IsNullOrEmpty(project.Deadline))
            {
                MessageBox.Show("Don't leave the fields empty");
                return false;
            }

            var response = Put<ProjectModel>($"/api/v2/projects/{projectSlug}", project);
            if (_responseHandler.CheckForErrorResponse(response))
            {
                return true;
            }

            MessageBox.Show("Error when updating a project please try again!");
            return false;
        }

        /// <summary>
        /// Handle api call to delete a project
        /// </summary>
        /// <param name="project">ProjectModel project</param>
        /// <returns>Boolean true if the call was a success false when something went wrong</returns>
        public bool DeleteProject(ProjectModel project)
        {
            var response = Delete($"/api/v2/projects/{project.ProjectSlug}");
            if (_responseHandler.CheckForErrorResponse(response))
            {
                return true;
            }

            MessageBox.Show("Error when deleting a project please try again!");
            return false;
        }

        /// <summary>
        /// Handle api call to get one project
        /// </summary>
        /// <param name="projectSlug">String with project slug</param>
        /// <returns> Single project model on success Null when something went wrong</returns>
        public SingleProjectModel GetOneProject(string projectSlug)
        {
            var response = Get($"/api/v2/projects/{projectSlug}");

            return _responseHandler.CheckForErrorResponse(response) ? _responseHandler.SingleObjectResponse<SingleProjectModel>(response, "Project") : null;
        }

        /// <summary>
        /// Add settings of a specific project to the database or update the data when the projectId already exists
        /// </summary>
        /// <param name="projectId">int projectId of current project</param>
        /// <param name="filePath">string with filepath of project</param>
        /// <param name="ideExec">string with the .exe file of the chosen IDE</param>
        public void AddSettingsToDb(int projectId, string filePath, string ideExec)
        {
            // Get connection string out of settings
            var cnString = Properties.Settings.Default.connectionString;
            // Make new SQL Connection
            var connection = new SqlConnection(cnString);
            // Open connection
            connection.Open();

            var sqlInsert = "";
            // SQL Query
            if (ideExec != string.Empty && filePath != string.Empty)
            {
                sqlInsert = "IF EXISTS (SELECT * FROM ides WHERE projectId='" + projectId + "') UPDATE ides SET filePath='" + filePath + "', ideExecutable='" + ideExec + "' WHERE projectId='" + projectId + "' ELSE BEGIN INSERT INTO ides (projectId, filePath, ideExecutable) VALUES('" + projectId + "', '" + filePath + "', '" + ideExec + "') END";
            }
            else if (filePath == string.Empty)
            {
                sqlInsert = "IF EXISTS (SELECT * FROM ides WHERE projectId='" + projectId + "') UPDATE ides SET ideExecutable='" + ideExec + "' WHERE projectId='" + projectId + "' ELSE BEGIN INSERT INTO ides (projectId, ideExecutable) VALUES('" + projectId + "', '" + ideExec + "') END";
            }
            else if (ideExec == string.Empty)
            {
                sqlInsert = "IF EXISTS (SELECT * FROM ides WHERE projectId='" + projectId + "') UPDATE ides SET filePath='" + filePath + "' WHERE projectId='" + projectId + "' ELSE BEGIN INSERT INTO ides (projectId, filePath) VALUES('" + projectId + "', '" + filePath + "') END";
            }
            var sqlCommand = new SqlCommand(sqlInsert, connection);
            // Execute SQL Command
            sqlCommand.ExecuteNonQuery();
            // Close connection            
            connection.Close();
        }

        /// <summary>
        /// Retrieve columns from database from a specific project
        /// </summary>
        /// <param name="projectId">int projectId from current project</param>
        /// <returns>IList with data from the database</returns>
        public IList<IdeModel> RetrieveFromDb(int projectId)
        {
            // Declare IList
            IList<IdeModel> openProjectSettings = new List<IdeModel>();
            // Set new SQL connection
            var connection = new SqlConnection(Properties.Settings.Default.connectionString);
            // Set SQL command to be executed
            var sqlCommand = new SqlCommand("SELECT * FROM ides WHERE projectId='" + projectId + "'", connection);
            try
            {
                // Open connection
                connection.Open();
                // Set new dataReader and execute reader with SQL command
                var dataReader = sqlCommand.ExecuteReader();
                // While reading the data gets added to the IList as a user model
                while (dataReader.Read())
                {
                    var projectIdInDb = (int)dataReader["projectId"];
                    if (projectIdInDb == projectId)
                    {
                        var ideExecutable = (string)dataReader["ideExecutable"];
                        var filePath = (string)dataReader["filePath"];
                        openProjectSettings.Add(new IdeModel(ideExecutable, filePath));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                connection.Close();
            }
            return openProjectSettings;
        }
    }
}
