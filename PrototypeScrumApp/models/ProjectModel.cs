using Newtonsoft.Json;
using System.Collections.Generic;

namespace PrototypeScrumApp.models
{
    public class ProjectModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("deadline")]
        public string Deadline { get; set; }

        [JsonProperty("users")]
        public IList<int> Users { get; set; } = new List<int>();

        [JsonProperty("slug")]
        public string ProjectSlug { get; set; }
    }
}
