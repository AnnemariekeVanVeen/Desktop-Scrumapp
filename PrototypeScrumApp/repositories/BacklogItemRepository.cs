using PrototypeScrumApp.helpers;
using PrototypeScrumApp.models;
using System.Collections.Generic;
using System.Windows;

namespace PrototypeScrumApp.repositories
{
    public class BacklogItemRepository : ApiRepository
    {
        private readonly ResponseHandler _responseHandler;

        public BacklogItemRepository()
        {
            _responseHandler = new ResponseHandler();
        }

        /// <summary>
        /// Handle add a new backlog item api call
        /// </summary>
        /// <param name="backlogItem"> A new backlog item model </param>
        /// <param name="projectSlug"> Slug of the project</param>
        /// <returns>Bool true on success. false when something went wrong</returns>
        public bool AddBacklogItem(BacklogItemModel backlogItem, string projectSlug)
        {
            if (string.IsNullOrEmpty(backlogItem.Action) || string.IsNullOrEmpty(backlogItem.UserRole))
            {
                MessageBox.Show("Don't leave the fields empty");
                return false;
            }

            var response = Post<BacklogItemModel>($"/api/v2/project/{projectSlug}/backlog_items", backlogItem);
            if (_responseHandler.CheckForErrorResponse(response))
            {
                return true;
            }

            MessageBox.Show("Something went wrong with Creating a backlog item please try again!");
            return false;
        }

        /// <summary>
        /// get one backlogitem from the api
        /// </summary>
        /// <param name="projectSlug"></param>
        /// <param name="backlogItemId"></param>
        /// <returns></returns>
        public BacklogItemModel GetOneBacklogItem(string projectSlug, int backlogItemId)
        {
            var response = Get($"/api/v2/project/{projectSlug}/backlog_items/{backlogItemId}");

            return _responseHandler.CheckForErrorResponse(response) ? _responseHandler.SingleObjectResponse<BacklogItemModel>(response, "BacklogItem") : null;
        }

        /// <summary>
        /// Get all the backlog items from the api
        /// </summary>
        /// <param name="projectSlug"> String of the project slug</param>
        /// <returns > IList of backlog items. null when something went wrong</returns>
        public IList<BacklogItemModel> GetAllBacklogItems(string projectSlug)
        {
            var response = Get($"/api/v2/project/{projectSlug}/backlog_items");

            return _responseHandler.CheckForErrorResponse(response) ? _responseHandler.ListOfObjectsResponse<BacklogItemModel>(response, "BacklogItems") : null;
        }

        /// <summary>
        /// Handle get all the backlog items api call
        /// </summary>
        /// <param name="projectSlug"> String of the project slug</param>
        /// <param name="sprintSlug"> String of the sprint slug</param>
        /// <returns > string of backlog items. null when something went wrong</returns>
        public string GetAllBacklogItemsForBoard(string projectSlug, string sprintSlug)
        {
            var response = Get($"/api/v2/project/{projectSlug}/kanban/{sprintSlug}/list");

            return _responseHandler.CheckForErrorResponse(response) ? response : null;
        }

        /// <summary>
        /// Handle get all the backlog item types api call
        /// </summary>
        /// <param name="apiPath"> Api path</param>
        /// <returns > IList of backlog types. null when something went wrong</returns>
        public IList<BacklogTypeModel> GetBacklogItemTypes()
        {
            var response = Get("/api/v2/backlogtypes");

            return _responseHandler.CheckForErrorResponse(response) ? _responseHandler.ListOfObjectsResponse<BacklogTypeModel>(response, "BacklogTypes") : null;
        }

        /// <summary>
        /// Handle update a backlog item with a new state from the board api call
        /// </summary>
        /// <param name="projectSlug"> Slug of the project</param>
        /// <param name="sprintSlug"> Slug of the sprint</param>
        /// <param name="stateModel"> State model that will update the item</param>
        /// <returns>Bool true on success. false when something went wrong</returns>
        public bool Update(string projectSlug, string sprintSlug, StateModel stateModel)
        {
            var response = Post<StateModel>($"/api/v2/project/{projectSlug}/kanban/{sprintSlug}/update", stateModel);

            if (_responseHandler.CheckForErrorResponse(response))
            {
                return true;
            }

            MessageBox.Show("Something went wrong with updating a backlog item please try again!");
            return false;
        }

        /// <summary>
        /// Handle update the backlog item api call
        /// </summary>
        /// <param name="projectSlug"> Slug of the project</param>
        /// <param name="backlogItemId"> Id of the backlog item </param>
        /// <param name="backlogItem"> Model of the backlog item to update to</param>
        /// <returns>Bool true on success. false when something went wrong</returns>
        public bool UpdateBacklogItem(string projectSlug, int backlogItemId, BacklogItemModel backlogItem)
        {
            if (string.IsNullOrEmpty(backlogItem.Action) || string.IsNullOrEmpty(backlogItem.UserRole))
            {
                MessageBox.Show("Don't leave the fields empty");
                return false;
            }

            var response = Put<BacklogItemModel>($"/api/v2/project/{projectSlug}/backlog_items/{backlogItemId}", backlogItem);

            if (_responseHandler.CheckForErrorResponse(response))
            {
                return true;
            }

            MessageBox.Show("Something went wrong with updating a backlog item please try again!");
            return false;
        }

        /// <summary>
        /// Handle delete sprint Api call 
        /// </summary>
        /// <param name="projectSlug"> Slug of the project</param>
        /// <param name="backlogItemId"> Id of the backlog item</param>
        /// <returns>Bool true on success. false when something went wrong</returns>
        public bool Delete(string projectSlug, string backlogItemId)
        {
            var response = Delete($"/api/v2/project/{projectSlug}/backlog_items/{backlogItemId}");

            if (_responseHandler.CheckForErrorResponse(response))
            {
                return true;
            }

            MessageBox.Show("Something went wrong with deleting a backlog item please try again!");
            return false;
        }
    }
}
