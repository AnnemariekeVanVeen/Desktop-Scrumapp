using Newtonsoft.Json;

namespace PrototypeScrumApp.models
{
    internal class ApiTokenModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("api_token")]
        public string ApiToken { get; set; }
    }
}
