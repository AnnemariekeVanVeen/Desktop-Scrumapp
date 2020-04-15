using Newtonsoft.Json;
using System.Collections.Generic;

namespace PrototypeScrumApp.models
{
    public class SingleProjectModel
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("deadline")]
        public string Deadline { get; set; }

        [JsonProperty("users")]
        public IList<UserModel> Users { get; set; } = new List<UserModel>();

        [JsonProperty("slug")]
        public string ProjectSlug { get; set; }
    }
}