using Newtonsoft.Json;

namespace PrototypeScrumApp.models
{
    public class StateModel
    {

        [JsonProperty("backlog_item_id")]
        public int BacklogItemId { get; set; }

        [JsonProperty("state_id")]
        public int StateId { get; set; }

        public StateModel(int id, int stateId)
        {
            BacklogItemId = id;
            StateId = stateId;
        }

        public StateModel()
        { }
    }
}
