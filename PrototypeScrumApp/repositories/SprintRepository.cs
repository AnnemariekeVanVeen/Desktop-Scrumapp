using PrototypeScrumApp.helpers;
using PrototypeScrumApp.models;
using System.Collections.Generic;
using System.Windows;

namespace PrototypeScrumApp.repositories
{
    public class SprintRepository : ApiRepository
    {
        private readonly ResponseHandler _responseHandler;

        public SprintRepository()
        {
            _responseHandler = new ResponseHandler();
        }

        /// <summary>
        /// Handle create a sprint api call
        /// </summary>
        /// <param name="sprint"> Model of the new sprint</param>
        /// <returns> Boolean true if the api call was a success false if there where complications</returns>
        public bool CreateSprint(SprintModel sprint)
        {
            if (string.IsNullOrEmpty(sprint.Name) || string.IsNullOrEmpty(sprint.StartDate) || string.IsNullOrEmpty(sprint.EndDate))
            {
                MessageBox.Show("Don't leave the fields empty");
                return false;
            }

            var response = Post<SprintModel>($"/api/v2/project/{sprint.ProjectSlug}/sprint", sprint);
            if (_responseHandler.CheckForErrorResponse(response))
            {
                return true;
            }

            MessageBox.Show("Error when creating a sprint please try again!");
            return false;
        }

        /// <summary>
        /// Handle get all sprints api call
        /// </summary>
        /// <param name="projectSlug"></param>
        /// <returns> String with the response of the api call</returns>
        public IList<SprintModel> GetSprints(string projectSlug)
        {
            var response = Get($"/api/v2/project/{projectSlug}/sprint");

            return _responseHandler.CheckForErrorResponse(response) ? _responseHandler.ListOfObjectsResponse<SprintModel>(response, "Sprints") : null;
        }

        /// <summary>
        /// Handle get one sprint api call
        /// </summary>
        /// <param name="projectSlug"> String with project slug</param>
        /// <param name="sprintSlug"> String with sprint slug</param>
        /// <returns> Sprint model on success. null if something went wrong</returns>
        public SprintModel GetOneSprint(string projectSlug, string sprintSlug)
        {
            var response = Get($"/api/v2/project/{projectSlug}/sprint/{sprintSlug}");

            return _responseHandler.CheckForErrorResponse(response) ? _responseHandler.SingleObjectResponse<SprintModel>(response, "Sprint") : null;
        }

        /// <summary>
        /// Handle get all backlog items from one sprint api call 
        /// </summary>
        /// <param name="projectSlug"> String with project slug</param>
        /// <param name="sprintSlug"> String with sprint slug</param>
        /// <returns> IList of backlog items from the sprint of the sprint slug. null if nothing is found</returns>
        public IList<BacklogItemModel> GetAllBacklogItemsOfOneSprint(string projectSlug, string sprintSlug)
        {
            var response = Get($"/api/v2/project/{projectSlug}/sprintplanning/{sprintSlug}/list");

            return _responseHandler.CheckForErrorResponse(response)
                ? _responseHandler.ListOfObjectsResponse<BacklogItemModel>(response, "SprintBacklog") : null;
        }

        /// <summary>
        /// Handle delete sprint api call
        /// </summary>
        /// <param name="sprintSlug"> Slug of the sprint</param>
        /// <param name="projectSlug"> Slug of the project</param>
        /// <returns> Boolean true if the api call was a success false if there where complications</returns>
        public bool DeleteSprint(string sprintSlug, string projectSlug)
        {
            var result = Delete($"/api/v2/project/{projectSlug}/sprint/{sprintSlug}");

            if (_responseHandler.CheckForErrorResponse(result))
            {
                return true;
            }

            MessageBox.Show("Error when deleting a sprint please try again!");
            return false;
        }

        /// <summary>
        /// Handle update a sprint api call
        /// </summary>
        /// <param name="sprint"> Updated sprint model</param>
        /// <returns> Boolean true if the api call was a success false if there where complications</returns>
        public bool UpdateSprint(SprintModel sprint)
        {
            if (string.IsNullOrEmpty(sprint.Name) || string.IsNullOrEmpty(sprint.StartDate) || string.IsNullOrEmpty(sprint.EndDate))
            {
                MessageBox.Show("Don't leave the fields empty");
                return false;
            }

            var response = Put<SprintModel>($"/api/v2/project/{sprint.ProjectSlug}/sprint/{sprint.SprintSlug}", sprint);
            if (_responseHandler.CheckForErrorResponse(response))
            {
                return true;
            }

            MessageBox.Show("Error when updating a sprint please try again!");
            return false;
        }

        /// <summary>
        /// Handle api call to add a backlog item to sprint
        /// </summary>
        /// <param name="sprintSlug"> String project slug</param>
        /// <param name="projectSlug"> String sprint slug</param>
        /// <param name="backlogItem"> Backlog item model</param>
        /// <returns> String response. null when something went wrong</returns>
        public string UpdateBacklogItem(string projectSlug, string sprintSlug, BacklogItemModel backlogItem)
        {
            var response = Post<BacklogItemModel>($"/api/v2/project/{projectSlug}/sprintplanning/{sprintSlug}/update", backlogItem);
            return _responseHandler.CheckForErrorResponse(response) ? response : null;
        }
    }
}
