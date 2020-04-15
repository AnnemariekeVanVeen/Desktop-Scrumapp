using Newtonsoft.Json;

namespace PrototypeScrumApp.models
{
    public class SprintModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        [JsonProperty("end_date")]
        public string EndDate { get; set; }

        [JsonProperty("slug")]
        public string SprintSlug { get; set; }

        [JsonProperty("project_slug")]
        public string ProjectSlug { get; set; }
    }
}