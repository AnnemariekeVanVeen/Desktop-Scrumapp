using PrototypeScrumApp.models;

namespace PrototypeScrumApp.factories
{
    public class SprintFactory
    {
        /// <summary>
        /// Make sprint model to create a new sprint
        /// </summary>
        /// <param name="name"> String name</param>
        /// <param name="projectId"> String project id</param>
        /// <param name="startDate"> String start date</param>
        /// <param name="endDate"> String end date</param>
        /// <returns> Sprint model to create a new sprint</returns>
        public SprintModel MakeCreateSprintModel(string name, string projectId, string startDate, string endDate, string projectSlug)
        {
            return new SprintModel()
            {
                Name = name,
                ProjectId = projectId,
                StartDate = startDate,
                EndDate = endDate,
                ProjectSlug = projectSlug
            };
        }

        /// <summary>
        /// Make sprint model to get a sprint
        /// </summary>
        /// <param name="name"> String name</param>
        /// <param name="projectId"> String project id</param>
        /// <param name="startDate"> String start date</param>
        /// <param name="endDate"> String end date</param>
        /// <param name="sprintSlug"> String sprint slug</param>
        /// <param name="projectSlug"> String project slug</param>
        /// <returns> Sprint model that you got from database</returns>
        public SprintModel MakeGetSingleSprintModel(string name, string projectId, string startDate, string endDate, string sprintSlug, string projectSlug)
        {
            return new SprintModel()
            {
                Name = name,
                ProjectId = projectId,
                StartDate = startDate,
                EndDate = endDate,
                SprintSlug = sprintSlug,
                ProjectSlug = projectSlug
            };
        }

        /// <summary>
        /// Make sprint model to update the sprint
        /// </summary>
        /// <param name="name"> String name</param>
        /// <param name="startDate"> String start date</param>
        /// <param name="endDate"> String end date</param>
        /// <returns> Sprint model that will be updated to</returns>
        public SprintModel MakeUpdateSprintModel(string name, string startDate, string endDate, string projectSlug, string sprintSlug)
        {
            return new SprintModel()
            {
                Name = name,
                StartDate = startDate,
                EndDate = endDate,
                ProjectSlug = projectSlug,
                SprintSlug = sprintSlug
            };
        }
    }
}
