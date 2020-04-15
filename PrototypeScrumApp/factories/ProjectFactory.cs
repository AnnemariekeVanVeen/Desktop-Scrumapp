using PrototypeScrumApp.models;
using System.Collections.Generic;

namespace PrototypeScrumApp.factories
{
    public class ProjectFactory
    {
        /// <summary>
        /// Make project model from a selected item
        /// </summary>
        /// <param name="title"> String title</param>
        /// <param name="description"> String description</param>
        /// <param name="deadline"> String deadline</param>
        /// <param name="slug"> String slug</param>
        /// <returns> Project model of the selected project item</returns>
        public ProjectModel MakeSelectedProjectModel(string title, string description, string deadline, string slug)
        {
            return new ProjectModel()
            {
                Title = title,
                Description = description,
                Deadline = deadline,
                ProjectSlug = slug
            };
        }

        /// <summary>
        /// Make project model for creating and updating the project
        /// </summary>
        /// <param name="title"> String title</param>
        /// <param name="description"> String description</param>
        /// <param name="deadline"> String deadline</param>
        /// <param name="users"> IList int users</param>
        /// <returns> Project model for creating or updating the project</returns>
        public ProjectModel MakeCreateAndUpdateProjectModel(string title, string description, string deadline,
            IList<int> users)
        {
            return new ProjectModel()
            {
                Title = title,
                Description = description,
                Deadline = deadline,
                Users = users
            };
        }
    }
}
