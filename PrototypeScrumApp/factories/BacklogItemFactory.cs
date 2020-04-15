using PrototypeScrumApp.models;

namespace PrototypeScrumApp.factories
{
    public class BacklogItemFactory
    {
        /// <summary>
        /// Make a backlog item model for creating a new item
        /// </summary>
        /// <param name="userRole"> String user role</param>
        /// <param name="action"> String action</param>
        /// <param name="storyPoints"> Int story points</param>
        /// <param name="backlogTypeId"> Int backlog type id</param>
        /// <returns> Backlog item model to create a new item</returns>
        public BacklogItemModel MakeNewBacklogItemModel(string userRole, string action, int storyPoints, int backlogTypeId)
        {
            return new BacklogItemModel()
            {
                UserRole = userRole,
                Action = action,
                StoryPoints = storyPoints,
                BacklogTypeId = backlogTypeId
            };
        }

        /// <summary>
        /// Make backlog item model to update the item
        /// </summary>
        /// <param name="id"> Int id</param>
        /// <param name="userRole"> String user role</param>
        /// <param name="action"> String action</param>
        /// <param name="storyPoints"> Int story points</param>
        /// <param name="backlogTypeId"> Int backlog type id</param>
        /// <param name="stateId"> Int state id</param>
        /// <param name="sprintId"> String sprint id</param>
        /// <returns> Backlog item model to update to</returns>
        public BacklogItemModel MakeUpdateBacklogItemModel(int id, string userRole, string action, int storyPoints, int backlogTypeId, int stateId, int sprintId)
        {
            return new BacklogItemModel()
            {
                BacklogItemId = id,
                UserRole = userRole,
                Action = action,
                StoryPoints = storyPoints,
                BacklogTypeId = backlogTypeId,
                StateId = stateId,
                SprintId = sprintId
            };
        }


        /// <summary>
        /// Make a backlog item model for adding a item to a sprint
        /// </summary>
        /// <param name="id"> Int for the id</param>
        /// <param name="inSprint"> Bool if it is in the sprint</param>
        /// <returns> Backlog item model for adding it to a sprint</returns>
        public static BacklogItemModel AddBacklogItem(int id, bool inSprint)
        {
            return new BacklogItemModel()
            {
                ItemId = id,
                InSprint = inSprint
            };
        }
    }
}
