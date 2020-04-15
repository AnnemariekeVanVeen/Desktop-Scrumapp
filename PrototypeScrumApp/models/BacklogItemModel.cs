using Newtonsoft.Json;

namespace PrototypeScrumApp.models
{
    public class BacklogItemModel
    {
        [JsonProperty("id")]
        public int BacklogItemId { get; set; }

        [JsonProperty("user_role")]
        public string UserRole { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("story_points")]
        public int StoryPoints { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("sprint_id")]
        public int? SprintId { get; set; }

        [JsonProperty("state_id")]
        public int StateId { get; set; }

        [JsonProperty("backlog_type_id")]
        public int BacklogTypeId { get; set; }

        [JsonProperty("backlog_type")]
        public BacklogTypeModel BacklogType { get; set; }

        [JsonProperty("in_sprint")]
        public bool InSprint { get; set; }

        [JsonProperty("backlog_item_id")]
        public int ItemId { get; set; }

        [JsonProperty("project_slug")]
        public string ProjectSlug { get; set; }
    }
}
